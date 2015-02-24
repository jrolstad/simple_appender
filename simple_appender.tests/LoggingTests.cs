using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Dynamic;
using System.IO;
using System.Linq;
using log4net;
using log4net.Config;
using log4net.Core;
using NUnit.Framework;

namespace simple_appender.tests
{
    [TestFixture]
    public class LoggingTests
    {
        [Test]
        public void LogInfo_nativeType_SendsMessage()
        {
            // Arrange
            var appender = new AmqpAppender { ExchangeName = "logging.test", ServerUri = "amqp://uat_teamdq:uat_teamdq@SEA-2600-49.paraport.com:5672", RoutingKey = null };
            appender.Layout = new JsonLayout();

            appender.ActivateOptions();
          
            BasicConfigurator.Configure(appender);

            var logger = LogManager.GetLogger(this.GetType());

            // Act
            logger.Info(1);

            // Assert
        }

        [Test]
        public void LogInfo_customType_SendsMessage()
        {
            // Arrange
            var appender = new AmqpAppender { ExchangeName = "logging.test", ServerUri = "amqp://uat_teamdq:uat_teamdq@SEA-2600-49.paraport.com:5672", RoutingKey = null };
            appender.Layout = new JsonLayout();

            appender.ActivateOptions();

            BasicConfigurator.Configure(appender);

            var logger = LogManager.GetLogger(this.GetType());

            // Act
            var item = new SomeType {Id = 1, Name = "My Name", UsedAt = DateTime.Now};
            logger.Info(item);

            // Assert
        }

        [Test]
        public void LogInfo_dynamic_SendsMessage()
        {
            // Arrange
            var appender = new AmqpAppender { ExchangeName = "logging.test", ServerUri = "amqp://uat_teamdq:uat_teamdq@SEA-2600-49.paraport.com:5672", RoutingKey = null };
            appender.Layout = new JsonLayout();

            appender.ActivateOptions();

            BasicConfigurator.Configure(appender);

            var logger = LogManager.GetLogger(this.GetType());

            // Act
            dynamic item = new ExpandoObject();
            item.Id = 2;
            item.Name = "some name";
            item.LastAccessedAt = DateTime.Now;

            logger.Info(item);

            // Assert
        }

        public class SomeType
        {
            public int Id;

            public string Name;

            public DateTime UsedAt;
        }

        
    }

   
}