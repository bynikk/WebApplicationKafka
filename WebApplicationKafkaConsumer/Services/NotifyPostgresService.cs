using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace WebApplicationKafkaConsumer.Services
{
    public class NotifyPostgresService : BackgroundService
    {
        const string connString = "Host=localhost;Port=5432;Database=kafkadb;Username=postgres;Password=123";
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await using var conn = new NpgsqlConnection(connString);
            await conn.OpenAsync();

            //e.Payload is string representation of JSON we constructed in NotifyOnDataChange() function
            conn.Notification += (o, e) => Console.WriteLine("Received notification: " + e.Payload);

            await using (var cmd = new NpgsqlCommand("LISTEN datachange;", conn))
                cmd.ExecuteNonQuery();

            while (true)
                conn.Wait(); // wait for events
        }
    }
}
