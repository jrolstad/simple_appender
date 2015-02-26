using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Json;
using log4net.Core;
using log4net.Layout;
using Newtonsoft.Json;

namespace simple_appender
{
    public class LogstashLayout : LayoutSkeleton
    {
        private readonly DataContractJsonSerializer _serializer = new DataContractJsonSerializer(typeof(LogstashEntry), new DataContractJsonSerializerSettings { UseSimpleDictionaryFormat = true });

        public override void ActivateOptions()
        {

        }

        public override void Format(TextWriter writer, LoggingEvent loggingEvent)
        {
            var logstashEntry = new LogstashEntry
            {
                application_name = Assembly.GetEntryAssembly().GetName().Name,
                machine_name = Environment.MachineName,
                user_name = loggingEvent.UserName,
                entry_date = loggingEvent.TimeStamp,
                performance = loggingEvent.MessageObject,
                error = loggingEvent.ExceptionObject
            };

            var message = JsonConvert.SerializeObject(logstashEntry, Formatting.None);
            writer.Write(message);
        }

    }

    public class LogstashEntry
    {
        public string application_name { get; set; }

        public string machine_name { get; set; }

        public string user_name { get; set; }

        public DateTime entry_date { get; set; }

        public object content { get; set; }

        public object performance { get; set; }

        public object error { get; set; }
    }
}