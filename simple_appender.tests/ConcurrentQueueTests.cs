using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace simple_appender.tests
{
    [TestFixture]
    public class ConcurrentQueueTests
    {
        [Test]
        public void PushingToQueueCanBeReadInBackground()
        {
            var queue = new BlockingCollection<string>();

            Action writer = () =>
            {
                foreach (var message in queue.GetConsumingEnumerable())
                {
                    Console.WriteLine(message);
                }
                
            };

            var factory = new TaskFactory();
            factory.StartNew(writer);
            
            var writingTask = new Task(writer);
            writingTask.Start();

            queue.TryAdd("foo");
            queue.TryAdd("one");
            queue.TryAdd("two");

            Thread.Sleep(1000);
        }

        
    }
}