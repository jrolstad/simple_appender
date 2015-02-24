using System.Collections.Generic;

namespace simple_appender.tests.Fakes.models
{
    public class Exchange
    {
        public string Name { get; set; } 
        public string Type { get; set; } 
        public bool IsDurable { get; set; } 
        public bool AutoDelete { get; set; } 
        public IDictionary<string,object> Arguments = new Dictionary<string, object>();
        public List<dynamic> PublishedMessages = new List<dynamic>();
    }
}