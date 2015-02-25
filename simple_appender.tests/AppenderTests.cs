using System;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using NUnit.Framework;

namespace simple_appender.tests
{
    [TestFixture]
    public class AppenderTests
    {
        [Test]
        public void LogInfo_OnDifferentThreads_AppenderLogsOnThread()
        {
            BasicConfigurator.Configure(new SimpleAppender());

            var log = LogManager.GetLogger(this.GetType());

            new Task(() => { log.Info(DateTime.Now); }).Start();
            new Task(() => { log.Info(DateTime.Now); }).Start();
            new Task(() => { log.Info(DateTime.Now); }).Start();
            new Task(() => { log.Info(DateTime.Now); }).Start();
            new Task(() => { log.Info(DateTime.Now); }).Start();

            Thread.Sleep(1000);
        }
    }

    public class SimpleAppender : AppenderSkeleton
    {
        protected override void Append(LoggingEvent loggingEvent)
        {
            Console.WriteLine("{0}|{1}",Thread.CurrentThread.ManagedThreadId,loggingEvent.RenderedMessage);
        }
    }


}