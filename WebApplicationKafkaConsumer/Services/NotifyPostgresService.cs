using Newtonsoft.Json;
using Npgsql;
using WebApplicationKafkaConsumer.CacheDistribution;
using WebApplicationKafkaConsumer.Entities;
using WebApplicationKafkaConsumer.Interfaces.CacheDistribution.Producers;

namespace WebApplicationKafkaConsumer.Services
{
    public class NotifyPostgresService : BackgroundService
    {
        const string connString = "Host=localhost;Port=5432;Database=kafkadb;Username=postgres;Password=123";

        IChannelProducer<OrderStreamModel> _producer;

        public NotifyPostgresService(IChannelProducer<OrderStreamModel> producer)
        {
            _producer = producer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await using var conn = new NpgsqlConnection(connString);
            await conn.OpenAsync();

            conn.Notification += (o, e) => ExecuteCacheCommand(e);

            await using (var cmd = new NpgsqlCommand("LISTEN datachange;", conn))
                cmd.ExecuteNonQuery();

            while (true)
                conn.Wait();
        }

        public void ExecuteCacheCommand(NpgsqlNotificationEventArgs e)
        {
            var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(e.Payload);
            Console.WriteLine("Received notification: " + e.Payload);

            var table = dict["table"].ToString();
            var command = dict["action"].ToString();
            var item = JsonConvert.DeserializeObject<OrderRequest>(dict["data"].ToString());

            OrderStreamModel orderStreamModel = new OrderStreamModel(command, item);

            _producer.Write(orderStreamModel);
        }
    }
}
