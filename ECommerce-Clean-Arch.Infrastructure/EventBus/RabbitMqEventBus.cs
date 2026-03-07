using System.Text;

using Newtonsoft.Json;

using RabbitMQ.Client;

namespace ECommerce_Clean_Arch.Infrastructure.EventBus;

public class RabbitMqEventBus
{
    private readonly ConnectionFactory _connectionFactory;

    public RabbitMqEventBus(ConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task PublishAsync(object obj)
    {
        await using var connection = await _connectionFactory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();
        await channel.QueueDeclareAsync(
            queue: obj.GetType().Name,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );
        string message = JsonConvert.SerializeObject(obj);
        await channel.BasicPublishAsync(
            exchange: "",
            routingKey: obj.GetType().Name,
            mandatory: true,
            basicProperties: new BasicProperties
            {
                Persistent = true,
                ContentType = "application/json",
                ContentEncoding = "utf-8"
            },
            Encoding.UTF8.GetBytes(message)
        );
    }
}