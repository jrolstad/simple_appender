using System.IO;
using System.Runtime.Serialization.Json;
using log4net.Core;
using log4net.Layout;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace simple_appender
{
    public class NewtonSoftJsonLayout : LayoutSkeleton
    {
        public override void ActivateOptions()
        {

        }

        public override void Format(TextWriter writer, LoggingEvent loggingEvent)
        {
            var message = JsonConvert.SerializeObject(loggingEvent.MessageObject,Formatting.None,new JavaScriptDateTimeConverter());
            writer.Write(message);
        }
    }
}