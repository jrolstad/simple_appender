using System.IO;
using System.Runtime.Serialization.Json;
using log4net.Core;
using log4net.Layout;

namespace simple_appender
{
    public class JsonLayout : LayoutSkeleton
    {
        public override void ActivateOptions()
        {

        }

        public override void Format(TextWriter writer, LoggingEvent loggingEvent)
        {
            var settings = new DataContractJsonSerializerSettings {UseSimpleDictionaryFormat = true};
            var serializer = new DataContractJsonSerializer(loggingEvent.MessageObject.GetType(),settings);

            var stream = new MemoryStream();
            serializer.WriteObject(stream,loggingEvent.MessageObject);

            stream.Position = 0;
            var streamReader = new StreamReader(stream);
            var formattedMessage = streamReader.ReadToEnd();
            writer.Write(formattedMessage);
        }

    }
}
