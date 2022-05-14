using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
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
        private List<OrderRequest> orders = null;

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OrderRequest orderRequest)
        {
            orders = FillOrders();
            string message = JsonSerializer.Serialize(orders);
            Console.WriteLine(message);
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

                    var result = producer.ProduceAsync(
                    topic,
                    new Message<Null, string>
                    {
                        Value = message
                    });
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

        private List<OrderRequest> FillOrders()
        {
            var orders = new List<OrderRequest>();
            for (int i = 1; i < 2000; i++)
            {
                orders.Add(new OrderRequest() { OrderId = i, Value = "order" });
            }
            return orders;
        }
    }
}
