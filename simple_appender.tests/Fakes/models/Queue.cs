using System.Collections.Generic;

namespace simple_appender.tests.Fakes.models
{
    public class Queue
    {
        public string Name { get; set; }

        public bool IsDurable { get; set; }

        public bool IsExclusive { get; set; }

        public bool IsAutoDelete { get; set; }

        public IDictionary<string, object> Arguments = new Dictionary<string, object>();

        public List<dynamic> Messages = new List<dynamic>();
    }
}