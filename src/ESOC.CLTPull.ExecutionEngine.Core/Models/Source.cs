using System.Collections.Generic;

namespace ESOC.CLTPull.ExecutionEngine.Core.Models
{
    public class Source
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string DataSourceName { get; set; }
        public string HostName { get; set; }
        public int Port { get; set; }
        public string Service { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public List<Query> Queries { get; set; }
    }
}
