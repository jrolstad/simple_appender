using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using log4net.Config;
using NUnit.Framework;

namespace simple_appender.tests
{
    [TestFixture]
    public class ConfigurationTests
    {
        [Test]
        public void HowToGetConfigurationData()
        {
            // Act
            var config = (NameValueCollection)ConfigurationManager.GetSection("PPASectionGroup/PPA.Data.Database.Cortex");

            // Assert
            var server = config["Server"];
            var database = config["database"];

            Assert.That(server.ToLower(),Is.StringContaining("cortexdb"));
            Assert.That(database.ToLower(),Is.StringContaining("cortex_"));
        } 
    }
}