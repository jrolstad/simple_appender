using System;
using System.IO;
using log4net.Core;
using log4net.Layout;
using Newtonsoft.Json;

namespace simple_appender
{
    public class LogstashLayout : LayoutSkeleton
    {
        public override void ActivateOptions()
        {

        }

        public override void Format(TextWriter writer, LoggingEvent loggingEvent)
        {
            var isPerformanceEntry = loggingEvent.MessageObject is PerformanceEntry;

            var logstashEntry = new LogstashEntry
            {
                ApplicationName = AppDomain.CurrentDomain.FriendlyName,
                MachineName = Environment.MachineName,
                UserName = loggingEvent.UserName,
                EntryDate = loggingEvent.TimeStamp.ToUniversalTime(),
                Performance = isPerformanceEntry? loggingEvent.MessageObject:null,
                Error = loggingEvent.ExceptionObject,
                Content = isPerformanceEntry? null: loggingEvent.MessageObject
            };

            var message = JsonConvert.SerializeObject(logstashEntry, Formatting.None);
            writer.Write(message);
        }

    }

    public class PerformanceEntry
    {
        [JsonProperty("process_name")]
        public string ProcessName { get; set; }
        [JsonProperty("process_id")]
        public string ProcessId { get; set; }

        [JsonProperty("parent_process_name")]
        public string ParentProcessName { get; set; }
        [JsonProperty("parent_process_id")]
        public string ParentProcessId { get; set; }

        [JsonProperty("event_type")]
        public string EventType { get; set; }
    }

    public class PerformanceEventType
    {
        public static string Enter = "enter";
        public static string Exit = "exit";
    }

    public class LogstashEntry
    {
        [JsonProperty("application_name")]
        public string ApplicationName { get; set; }

        [JsonProperty("machine_name")]
        public string MachineName { get; set; }

        [JsonProperty("user_name")]
        public string UserName { get; set; }

        [JsonProperty("entry_date")]
        public DateTime EntryDate { get; set; }

        [JsonProperty("content")]
        public object Content { get; set; }

        [JsonProperty("performance")]
        public object Performance { get; set; }

        [JsonProperty("error")]
        public object Error { get; set; }
    }
}