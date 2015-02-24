using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace simple_appender.tests.Fakes
{
    public class FakeModel:IModel
    {
        public Dictionary<string, List<dynamic>> PublishedMessages = new Dictionary<string, List<dynamic>>();

        public List<dynamic> PublishedMessagesOnExchange(string exchangeName)
        {
            return PublishedMessages.ContainsKey(exchangeName) ? 
                PublishedMessages[exchangeName] : new List<dynamic>();
        }

        public List<dynamic> AcknowledgedMessages = new List<dynamic>(); 
        public List<dynamic> RejectedMessages = new List<dynamic>();
        public List<dynamic> NonAcknowledgedMessages = new List<dynamic>();

        public bool ApplyPrefetchToAllChannels { get; private set; }
        public ushort PrefetchCount { get; private set; }
        public uint PrefetchSize { get; private set; }

        public void Dispose()
        {
            
        }

        public IBasicProperties CreateBasicProperties()
        {
            throw new NotImplementedException();
        }

        public void ExchangeDeclare(string exchange, string type, bool durable, bool autoDelete, IDictionary<string, object> arguments)
        {
            throw new NotImplementedException();
        }

        public void ExchangeDeclare(string exchange, string type, bool durable)
        {
            throw new NotImplementedException();
        }

        public void ExchangeDeclare(string exchange, string type)
        {
            throw new NotImplementedException();
        }

        public void ExchangeDeclarePassive(string exchange)
        {
            throw new NotImplementedException();
        }

        public void ExchangeDeclareNoWait(string exchange, string type, bool durable, bool autoDelete, IDictionary<string, object> arguments)
        {
            throw new NotImplementedException();
        }

        public void ExchangeDelete(string exchange, bool ifUnused)
        {
            throw new NotImplementedException();
        }

        public void ExchangeDelete(string exchange)
        {
            throw new NotImplementedException();
        }

        public void ExchangeDeleteNoWait(string exchange, bool ifUnused)
        {
            throw new NotImplementedException();
        }

        public void ExchangeBind(string destination, string source, string routingKey, IDictionary<string, object> arguments)
        {
            throw new NotImplementedException();
        }

        public void ExchangeBind(string destination, string source, string routingKey)
        {
            throw new NotImplementedException();
        }

        public void ExchangeBindNoWait(string destination, string source, string routingKey, IDictionary<string, object> arguments)
        {
            throw new NotImplementedException();
        }

        public void ExchangeUnbind(string destination, string source, string routingKey, IDictionary<string, object> arguments)
        {
            throw new NotImplementedException();
        }

        public void ExchangeUnbind(string destination, string source, string routingKey)
        {
            throw new NotImplementedException();
        }

        public void ExchangeUnbindNoWait(string destination, string source, string routingKey, IDictionary<string, object> arguments)
        {
            throw new NotImplementedException();
        }

        public QueueDeclareOk QueueDeclare()
        {
            throw new NotImplementedException();
        }

        public QueueDeclareOk QueueDeclarePassive(string queue)
        {
            throw new NotImplementedException();
        }

        public QueueDeclareOk QueueDeclare(string queue, bool durable, bool exclusive, bool autoDelete, IDictionary<string, object> arguments)
        {
            throw new NotImplementedException();
        }

        public void QueueDeclareNoWait(string queue, bool durable, bool exclusive, bool autoDelete, IDictionary<string, object> arguments)
        {
            throw new NotImplementedException();
        }

        public void QueueBind(string queue, string exchange, string routingKey, IDictionary<string, object> arguments)
        {
            throw new NotImplementedException();
        }

        public void QueueBind(string queue, string exchange, string routingKey)
        {
            throw new NotImplementedException();
        }

        public void QueueBindNoWait(string queue, string exchange, string routingKey, IDictionary<string, object> arguments)
        {
            throw new NotImplementedException();
        }

        public void QueueUnbind(string queue, string exchange, string routingKey, IDictionary<string, object> arguments)
        {
            throw new NotImplementedException();
        }

        public uint QueuePurge(string queue)
        {
            throw new NotImplementedException();
        }

        public uint QueueDelete(string queue, bool ifUnused, bool ifEmpty)
        {
            throw new NotImplementedException();
        }

        public void QueueDeleteNoWait(string queue, bool ifUnused, bool ifEmpty)
        {
            throw new NotImplementedException();
        }

        public uint QueueDelete(string queue)
        {
            throw new NotImplementedException();
        }

        public void ConfirmSelect()
        {
            throw new NotImplementedException();
        }

        public bool WaitForConfirms()
        {
            throw new NotImplementedException();
        }

        public bool WaitForConfirms(TimeSpan timeout)
        {
            throw new NotImplementedException();
        }

        public bool WaitForConfirms(TimeSpan timeout, out bool timedOut)
        {
            throw new NotImplementedException();
        }

        public void WaitForConfirmsOrDie()
        {
            throw new NotImplementedException();
        }

        public void WaitForConfirmsOrDie(TimeSpan timeout)
        {
            throw new NotImplementedException();
        }

        public string BasicConsume(string queue, bool noAck, IBasicConsumer consumer)
        {
            throw new NotImplementedException();
        }

        public string BasicConsume(string queue, bool noAck, string consumerTag, IBasicConsumer consumer)
        {
            throw new NotImplementedException();
        }

        public string BasicConsume(string queue, bool noAck, string consumerTag, IDictionary<string, object> arguments, IBasicConsumer consumer)
        {
            throw new NotImplementedException();
        }

        public string BasicConsume(string queue, bool noAck, string consumerTag, bool noLocal, bool exclusive, IDictionary<string, object> arguments,
            IBasicConsumer consumer)
        {
            throw new NotImplementedException();
        }

        public BasicGetResult BasicGet(string queue, bool noAck)
        {
            throw new NotImplementedException();
        }

        public void BasicCancel(string consumerTag)
        {
            throw new NotImplementedException();
        }

        public void BasicQos(uint prefetchSize, ushort prefetchCount, bool global)
        {
            PrefetchSize = prefetchSize;
            PrefetchCount = prefetchCount;
            ApplyPrefetchToAllChannels = global;
        }

        public void BasicPublish(PublicationAddress addr, IBasicProperties basicProperties, byte[] body)
        {
            if (!PublishedMessages.ContainsKey(addr.ExchangeName))
            {
                PublishedMessages.Add(addr.ExchangeName,new List<dynamic>());
            }

            dynamic parameters = new ExpandoObject();
            parameters.addr = addr;
            parameters.basicProperties = basicProperties;
            parameters.body = body;

            PublishedMessages[addr.ExchangeName].Add(parameters);
        }

        public void BasicPublish(string exchange, string routingKey, IBasicProperties basicProperties, byte[] body)
        {
            if (!PublishedMessages.ContainsKey(exchange))
            {
                PublishedMessages.Add(exchange, new List<dynamic>());
            }

            dynamic parameters = new ExpandoObject();
            parameters.exchange = exchange;
            parameters.routingKey = routingKey;
            parameters.basicProperties = basicProperties;
            parameters.body = body;

            PublishedMessages[exchange].Add(parameters);
        }

        public void BasicPublish(string exchange, string routingKey, bool mandatory, IBasicProperties basicProperties, byte[] body)
        {
            if (!PublishedMessages.ContainsKey(exchange))
            {
                PublishedMessages.Add(exchange, new List<dynamic>());
            }

            dynamic parameters = new ExpandoObject();
            parameters.exchange = exchange;
            parameters.routingKey = routingKey;
            parameters.mandatory = mandatory;
            parameters.basicProperties = basicProperties;
            parameters.body = body;

            PublishedMessages[exchange].Add(parameters);
        }

        public void BasicPublish(string exchange, string routingKey, bool mandatory, bool immediate, IBasicProperties basicProperties,byte[] body)
        {
            if (!PublishedMessages.ContainsKey(exchange))
            {
                PublishedMessages.Add(exchange, new List<dynamic>());
            }

            dynamic parameters = new ExpandoObject();
            parameters.exchange = exchange;
            parameters.routingKey = routingKey;
            parameters.mandatory = mandatory;
            parameters.immediate = immediate;
            parameters.basicProperties = basicProperties;
            parameters.body = body;

            PublishedMessages[exchange].Add(parameters);
        }

        public void BasicAck(ulong deliveryTag, bool multiple)
        {
            dynamic parameters = new ExpandoObject();
            parameters.deliveryTag = deliveryTag;
            parameters.multiple = multiple;

            AcknowledgedMessages.Add(parameters);
        }

        public void BasicReject(ulong deliveryTag, bool requeue)
        {
            dynamic parameters = new ExpandoObject();
            parameters.deliveryTag = deliveryTag;
            parameters.requeue = requeue;

            RejectedMessages.Add(parameters);
        }

        public void BasicNack(ulong deliveryTag, bool multiple, bool requeue)
        {
            dynamic parameters = new ExpandoObject();
            parameters.deliveryTag = deliveryTag;
            parameters.multiple = multiple;
            parameters.requeue = requeue;

            NonAcknowledgedMessages.Add(parameters);
        }

        public void BasicRecover(bool requeue)
        {
            throw new NotImplementedException();
        }

        public void BasicRecoverAsync(bool requeue)
        {
            throw new NotImplementedException();
        }

        public void TxSelect()
        {
            throw new NotImplementedException();
        }

        public void TxCommit()
        {
            throw new NotImplementedException();
        }

        public void TxRollback()
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            IsClosed = true;
            IsOpen = false;
        }

        public void Close(ushort replyCode, string replyText)
        {
            IsClosed = true;
            IsOpen = false;
        }

        public void Abort()
        {
            throw new NotImplementedException();
        }

        public void Abort(ushort replyCode, string replyText)
        {
            throw new NotImplementedException();
        }

        public IBasicConsumer DefaultConsumer { get; set; }

        public ShutdownEventArgs CloseReason { get; set; }

        public bool IsOpen { get; set; }

        public bool IsClosed { get; set; }

        public ulong NextPublishSeqNo { get; set; }

        public event ModelShutdownEventHandler ModelShutdown;
        public event BasicReturnEventHandler BasicReturn;
        public event BasicAckEventHandler BasicAcks;
        public event BasicNackEventHandler BasicNacks;
        public event CallbackExceptionEventHandler CallbackException;
        public event FlowControlEventHandler FlowControl;
        public event BasicRecoverOkEventHandler BasicRecoverOk;
    }
}