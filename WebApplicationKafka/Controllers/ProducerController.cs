using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Text.Json;
using WebApplicationKafkaProducer.Entities;

namespace WebApplicationKafkaProducer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProducerController : ControllerBase
    {
        private readonly string bootstrapServers = "localhost:9092";
        private readonly string topic = "test";

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OrderRequest orderRequest)
        {
            string message = JsonSerializer.Serialize(orderRequest);
            return Ok(await SendOrderRequest(topic, message));
        }
        private async Task<bool> SendOrderRequest(string topic, string message)
        {
            ProducerConfig config = new ProducerConfig
            {
                BootstrapServers = bootstrapServers,
                ClientId = Dns.GetHostName()
            };

            try
            {
                using (var producer = new ProducerBuilder<Null, string>(config).Build())
                {
                    var date = DateTime.UtcNow;
                    for (int i = 0; i < 2_000; i++)
                    {
                        var result = producer.ProduceAsync(
                        topic,
                        new Message<Null, string>
                        {
                            Value = message
                        });
                        Console.WriteLine($"#{i} Delivery Timestamp:");
                    }

                    var resultDate = (DateTime.UtcNow - date).Duration();
                    Console.WriteLine(resultDate);

                    return await Task.FromResult(true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occured: {ex.Message}");
            }

            return await Task.FromResult(false);
        }
    }
}
