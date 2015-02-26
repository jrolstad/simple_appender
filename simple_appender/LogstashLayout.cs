﻿using System;
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
        public override void ActivateOptions()
        {

        }

        public override void Format(TextWriter writer, LoggingEvent loggingEvent)
        {
            var isPerformanceEntry = loggingEvent.MessageObject is PerformanceEntry;

            var logstashEntry = new LogstashEntry
            {
                application_name = AppDomain.CurrentDomain.FriendlyName,
                machine_name = Environment.MachineName,
                user_name = loggingEvent.UserName,
                entry_date = loggingEvent.TimeStamp.ToUniversalTime(),
                performance = isPerformanceEntry? loggingEvent.MessageObject:null,
                error = loggingEvent.ExceptionObject,
                content = isPerformanceEntry? null: loggingEvent.MessageObject
            };

            var message = JsonConvert.SerializeObject(logstashEntry, Formatting.None);
            writer.Write(message);
        }

    }

    public class PerformanceEntry
    {
        public string process_name { get; set; }
        public string process_id { get; set; }

        public string parent_process_name { get; set; }
        public string parent_process_id { get; set; }

        public string event_type { get; set; }
    }

    public class PerformanceEventType
    {
        public static string Enter = "enter";
        public static string Exit = "exit";
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