using Confluent.Kafka;
using System.Diagnostics;
using System.Text.Json;
using WebApplicationKafkaConsumer.Entities;
using WebApplicationKafkaConsumer.Interfaces;

namespace WebApplicationKafkaConsumer.Services
{
    public class ApacheKafkaConsumerService : IHostedService
    {
        IRepository<OrderProcessingRequest> _repository;
        IServiceProvider _serviceProvider;
        private readonly string topic = "test";
        private readonly string groupId = "test_group";
        private readonly string bootstrapServers = "localhost:9092";
        int i = 0;

        public ApacheKafkaConsumerService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
         
        public Task StartAsync(CancellationToken cancellationToken)
        {
            var config = new ConsumerConfig
            {
                GroupId = groupId,
                BootstrapServers = bootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            try
            {
                using (var consumerBuilder = new ConsumerBuilder<Ignore, string>(config).Build())
                {
                    consumerBuilder.Subscribe(topic);
                    var cancelToken = new CancellationTokenSource();

                    try
                    {
                        while (true)
                        {
                            var consumer = consumerBuilder.Consume(cancelToken.Token);
                            var orderRequest = JsonSerializer.Deserialize<List<OrderProcessingRequest>>(consumer.Message.Value);
                            i++;
                            Console.WriteLine($"#{i} Processing Orders count:{ orderRequest.Count}");
                            AddOrdersToDb(orderRequest);
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        consumerBuilder.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return Task.CompletedTask;
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private async void AddOrdersToDb(List<OrderProcessingRequest> orders)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                _repository = scope.ServiceProvider.GetRequiredService<IRepository<OrderProcessingRequest>>();

                var date = DateTime.UtcNow;
                Console.WriteLine("Start add");
                foreach (var item in orders)
                {
                    Console.WriteLine("adding " + item.OrderId);
                    await _repository.Add(item);
                }
                var resultDate = (DateTime.UtcNow - date).Duration();
                Console.WriteLine(resultDate);
            }
        }
    }
}
