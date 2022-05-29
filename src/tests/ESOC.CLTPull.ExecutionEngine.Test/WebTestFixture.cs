using ESOC.CLTPull.WebApi;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace ESOC.CLTPull.ExecutionEngine.Test
{

    public class WebTestFixture : WebApplicationFactory<Startup>
    {
        private const string CORS_POLICY = "AllowOrigin";
        private readonly IConfiguration _configuration;

       //// protected HttpClient TestHttpClient => CreateClient();
        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>();
        }

    }
}
