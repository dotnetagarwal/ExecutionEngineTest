namespace ESOC.CLTPull.WebApi
{
    using ESOC.CLTPull.ExecutionEngine.API.Models;
    using ESOC.CLTPull.ExecutionEngine.Core.Contracts;
    using ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine;
    using ESOC.CLTPull.ExecutionEngine.DataExtractionRules;
    using HealthChecks.UI.Client;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Diagnostics.HealthChecks;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerUI;
    using System;
    using System.Linq;
    using ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine.Contract;
    using ESOC.CLTPull.ExecutionEngine.ResultProcessing;
    using ESOC.CLTPull.ExecutionEngine.Core.Entities.ExecutionEngine.Enums;
    using ESOC.CLT.ExecutionEngine.Infrastructure.Data;
    using ESOC.CLT.ExecutionEngine.Infrastructure.Data.AbstractFactory;
    using ESOC.CLTPull.ExecutionEngine.DataExtractionRules.OracleRules;
    using ESOC.CLTPull.ExecutionEngine.Alerting;
    using ESOC.CLTPull.WebAPI.Models;
    using System.IO;
    using Shared.Kafka.Producer;
    using Confluent.Kafka;
    using Shared.Kafka.Consumer;
    using EventBus;
    using Microsoft.Extensions.Logging;
    using ESOC.CLT.ExecutionEngine.Infrastructure.Data.Services;
    using ESOC.CLTPull.ExecutionEngine.Core.Models;
    using ESOC.CLTPull.ExecutionEngine.Core.EventHandlers;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using System.Reflection;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public virtual IServiceProvider ConfigureServices(IServiceCollection services)
        {

            //TODO : uncomment for jwt authentication
            // ConfigureAuthService(services);
            services.AddControllers();
            //AddCustomDbContext(services, Configuration);


            RegisterKafkaBus(services);

            services.AddScoped<INASApplyDataExtractionRules, NASApplyDataExtractionRules>();
            services.AddScoped<ISqlApplyDataExtractionRules, SqlApplyDataExtractionRules>();
            services.AddScoped<IOracleApplyDataExtractionRules, OracleApplyDataExtractionRules>();
            services.AddScoped<INASConnection, NASConnection>();
            services.AddScoped<IDbConnectionFactory, SharedConnection>();
            services.AddScoped<IRelationDbFactory, RelationDbFactory>();
            services.AddScoped<IOOBAttachment, OOBAttachment>();

            services.AddScoped<IControlExecutionResultsService, ControlExecutionResultsService>();
            services.AddScoped<ICompareObjectsService, CompareObjectsService>();
            services.AddScoped<IDataExtractionRulesVisitor, DataExtractionRulesVisitor>();
            services.AddScoped<IBusinessRulesService, BusinessRulesService>();

            services.AddTransient<IProcessControlsResult, RecordLevelControlResultProcessing>();
            services.AddTransient<IProcessControlsResult, ThresholdControlResultProcessing>();
            services.AddTransient<IProcessControlsResult, EmailControlResultProcessing>();
            services.AddTransient<IProcessControlsResult, TimlinessControlResultProcessing>();
            //services.AddTransient<IControlResult, RecordLevelControlResult>();
            services.AddTransient<IKafkaAlertingService, KafkaAlertingService>();

            services.AddTransient<RecordLevelControlResultProcessing>();
            services.AddTransient<ThresholdControlResultProcessing>();
            services.AddTransient<EmailControlResultProcessing>();
            services.AddTransient<TimlinessControlResultProcessing>();


            services.AddTransient<IControlResult, RecordLevelControlResult>();
            services.AddTransient<IControlResult, ThresholdControlResult>();
            services.AddTransient<RecordLevelControlResult>();
            services.AddTransient<ThresholdControlResult>();
            services.AddTransient<ControlExecutionEventHandler>();

            //TODO Enums or Dictionary for Case

            services.AddTransient<Func<String, dynamic>>(serviceProvider => sourceName =>
            {
                switch (sourceName)
                {
                    case "Sql":
                        return serviceProvider.GetRequiredService<ISqlApplyDataExtractionRules>();
                    case "NAS":
                        return serviceProvider.GetService<INASApplyDataExtractionRules>();
                    case "Oracle":
                        return serviceProvider.GetService<IOracleApplyDataExtractionRules>();

                    default:
                        return serviceProvider.GetService<ISqlApplyDataExtractionRules>();
                }
            });


            //TODO Different structure of Result, it should be detach from LHS and RHS 
            //<Resultsetstructure>
            //  <LHSResult>
            //   <Result>

            //For now I am passing "Result" as an input to Process Our Results

            services.AddTransient<Func<ControlTypes, IProcessControlsResult>>(serviceProvider => controlTypes =>
            {
                switch (controlTypes)
                {
                    case ControlTypes.RecordLevel:
                        return serviceProvider.GetService<RecordLevelControlResultProcessing>();
                    case ControlTypes.Threshold:
                        return serviceProvider.GetService<ThresholdControlResultProcessing>();
                    case ControlTypes.Timliness:
                        return serviceProvider.GetService<TimlinessControlResultProcessing>();
                    case ControlTypes.Email:
                        return serviceProvider.GetService<EmailControlResultProcessing>();
                    default:
                        return serviceProvider.GetService<RecordLevelControlResultProcessing>(); //TODO We need to implement a handler when control type not found
                }
            });

            services.AddTransient<Func<ControlTypes, IControlResult>>(serviceProvider => controlTypes =>
            {
                switch (controlTypes)
                {
                    case ControlTypes.RecordLevel:
                        return serviceProvider.GetService<RecordLevelControlResult>();
                    case ControlTypes.Threshold:
                        return serviceProvider.GetService<ThresholdControlResult>();
                    default:
                        return serviceProvider.GetService<RecordLevelControlResult>(); //TODO We need to implement a handler when control type not found
                }
            });

            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo { Version = "v1", Title = "Execution API" });
                swagger.ResolveConflictingActions(action => action.First());
            });


            services.AddCustomHealthCheck(Configuration);

            var container = new ContainerBuilder();
            container.RegisterAssemblyTypes(typeof(ControlExecutionEventHandler).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IIntegrationEventHandler<>));
            container.Populate(services);
            return new AutofacServiceProvider(container.Build());

        }

        #region Register KafkaBus 
        private void RegisterKafkaBus(IServiceCollection services)
        {
            var kafkaConfig = new KafkaConfig();
            Configuration.GetSection(nameof(KafkaConfig)).Bind(kafkaConfig);
            string caPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, kafkaConfig.CaPath);
            string certPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, kafkaConfig.CertPath);
            string keyPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, kafkaConfig.KeyPath);

            //TODO : Read the config section from appsettings.json
            var producerConfig = new KafkaProducerConfig
            {
                Topic = kafkaConfig.EEPublisherTopic,
                BootstrapServers = kafkaConfig.BootstrapServer,
                SecurityProtocol = SecurityProtocol.Ssl,
                SslCaLocation = caPath,
                SslCertificateLocation = certPath,
                SslKeyLocation = keyPath,
                Acks = Acks.All,
                // Number of times to retry before giving up
                MessageSendMaxRetries = kafkaConfig.MessageSendMaxRetries,
                // Duration to retry before next attempt
                RetryBackoffMs = kafkaConfig.RetryBackoffMs,
                // Set to true if you don't want to reorder messages on retry
                EnableIdempotence = kafkaConfig.EnableIdempotence
            };
            KafkaConsumerConfig consumerConfig = new KafkaConsumerConfig
            {
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false,
                EnablePartitionEof = true,
                Topic = kafkaConfig.Topic,
                BootstrapServers = kafkaConfig.BootstrapServer,
                SecurityProtocol = SecurityProtocol.Ssl,
                SslCaLocation = caPath,
                SslCertificateLocation = certPath,
                SslKeyLocation = keyPath,
                GroupId = "esoc-ctc.ExecutionEngineService",
                
            };
            services.AddSingleton<ConsumerConfig>(consumerConfig);
            services.AddSingleton(new KafkaEventBus.KafkaConnection(producerConfig, consumerConfig));
            services.AddSingleton<IEventBusSubscriptionManager, EventBusSubscriptionManager>();

            services.AddSingleton<IEventBus, KafkaEventBus.KafkaEventBus>(sp =>
            {
                var kafkaConnection = sp.GetRequiredService<KafkaEventBus.KafkaConnection>();
                var logger = sp.GetRequiredService<ILogger<KafkaEventBus.KafkaEventBus>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionManager>();
                return new KafkaEventBus.KafkaEventBus(eventBusSubcriptionsManager, logger, kafkaConnection, sp, consumerConfig,producerConfig);
            });
        }
        #endregion

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //app.Use(async (h, n) =>
            //{
            //    LogContext.PushProperty("ServiceName", nameof(ControlExecutionResultsService));
            //    await n.Invoke();
            //});
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                endpoints.MapHealthChecks("/liveness", new HealthCheckOptions
                {
                    Predicate = r => r.Name.Contains("self")
                });
            });

            UseSwaggerDocumentation(app);

            ConfigureEventBus(app);
        }

        protected virtual void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.AddSubscriptionAsync<ControlExecutionEvent, ControlExecutionEventHandler>();
            eventBus.SubscribeAsync(typeof(ControlExecutionEvent).Namespace);
        }

        private void UseSwaggerDocumentation(IApplicationBuilder app)
        {
            var swaggeroptions = new SwaggerSettings();
            Configuration.GetSection(nameof(SwaggerSettings)).Bind(swaggeroptions);
            app.UseSwagger();
            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint(swaggeroptions.UIEndPoint, swaggeroptions.Description);
                option.DocExpansion(DocExpansion.None);
            });
        }
    }

    public static class CustomExtensionMethods
    {
        public static IServiceCollection AddCustomHealthCheck(this IServiceCollection services, IConfiguration configuration)
        {
            var hcBuilder = services.AddHealthChecks();

            hcBuilder.AddCheck("ExecutionEngineAPI", () => HealthCheckResult.Healthy());

            return services;
        }
    }
}


