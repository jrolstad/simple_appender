using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Json;
using log4net.Core;
using log4net.Layout;

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
                entry_date = loggingEvent.TimeStamp

            };
            var stream = new MemoryStream();
            _serializer.WriteObject(stream, logstashEntry);

            stream.Position = 0;
            var streamReader = new StreamReader(stream);
            var formattedMessage = streamReader.ReadToEnd();
            writer.Write(formattedMessage);
        }

    }

    public class LogstashEntry
    {
        public string application_name { get; set; }

        public string machine_name { get; set; }

        public string user_name { get; set; }

        public DateTime entry_date { get; set; }

        public object message { get; set; }
    }
}