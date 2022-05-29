namespace ESOC.CLTPull.WebAPI.Models
{
    public class KafkaConfig
    {
        public string CaPath { get; set; }
        public string CertPath { get; set; }
        public string KeyPath { get; set; }
        public string Topic { get; set; }
        public string EEPublisherTopic { get; set; }
        public string BootstrapServer { get; set; }
        public int MessageSendMaxRetries { get; set; }
        public int RetryBackoffMs { get; set; }
        public bool EnableIdempotence { get; set; }

    }
}
