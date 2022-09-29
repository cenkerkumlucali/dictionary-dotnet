using System.Text;
using System.Text.Json;
using Common.Models.RequestModels;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Common.Infrastructure;

public static class QueueFactory
{
    public static void SendMessageToExchange(string exchangeName,
        string exchangeType,
        string queueName,
        object obj)
    {
        var channel = CreateBasicConsumer()
            .EnsureExchange(exchangeName,exchangeType)
            .EnsureQueue(queueName,exchangeName)
            .Model;
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(obj));
        channel.BasicPublish(exchange:exchangeName,
                             routingKey:queueName,
                             basicProperties:null,
                             body:body);
    }

    public static EventingBasicConsumer CreateBasicConsumer()
    {
        var factory = new ConnectionFactory { HostName = DictionaryConstants.RabbitMQHost };
        var connection = factory.CreateConnection();
        var channel = connection.CreateModel();
        return new EventingBasicConsumer(channel);
    }

    public static EventingBasicConsumer EnsureExchange(this EventingBasicConsumer consumer, string exchangeName,
        string exchangeType = DictionaryConstants.DefaultExchangeType)
    {
        consumer.Model.ExchangeDeclare(exchangeName, exchangeType, false, false);
        return consumer;
    }

    public static EventingBasicConsumer EnsureQueue(this EventingBasicConsumer consumer, string queueName,
        string exchangeName)
    {
        consumer.Model.QueueDeclare(queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        consumer.Model.QueueBind(queueName,exchangeName,queueName);
        return consumer;
    }
}