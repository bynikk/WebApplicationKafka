using Confluent.Kafka;
using System.Diagnostics;
using System.Text.Json;
using WebApplicationKafkaConsumer.Entities;
using WebApplicationKafkaConsumer.Interfaces;
using WebApplicationKafkaConsumer.Interfaces.Repositories;

namespace WebApplicationKafkaConsumer.Services
{
    public class ApacheKafkaConsumerService : BackgroundService
    {
        IRepository<OrderRequest> _repository;
        IServiceProvider _serviceProvider;
        private readonly string topic = "test";
        private readonly string groupId = "test_group";
        private readonly string bootstrapServers = "localhost:9092";
        Stopwatch _stopwatch = new();

        public ApacheKafkaConsumerService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
         
        protected override Task ExecuteAsync(CancellationToken cancellationToken)
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
                            _stopwatch.Start();
                            var orderRequest = JsonSerializer.Deserialize<List<OrderRequest>>(consumer.Message.Value);

                            Console.WriteLine("Before adding : " + _stopwatch.ElapsedMilliseconds);

                            AddOrdersToDb(orderRequest);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
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

        private async void AddOrdersToDb(List<OrderRequest> orders)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                _repository = scope.ServiceProvider.GetRequiredService<IRepository<OrderRequest>>();

                List<Task> tasks = new ();

                foreach (var item in orders)
                {
                    lock(_repository)
                    {
                        tasks.Add(_repository.Add(item));
                    }
                }
                Task.WaitAll(tasks.ToArray());

                await _repository.SaveChanges();

                _stopwatch.Stop();
                Console.WriteLine("full time : " + _stopwatch.ElapsedMilliseconds);
                _stopwatch.Reset();
            }
        }
    }
}
