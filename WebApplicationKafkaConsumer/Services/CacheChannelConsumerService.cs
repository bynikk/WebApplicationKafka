using WebApplicationKafkaConsumer.CacheDistribution;
using WebApplicationKafkaConsumer.Entities;
using WebApplicationKafkaConsumer.Interfaces.CacheDistribution;
using WebApplicationKafkaConsumer.Interfaces.CacheDistribution.Producers;

namespace WebApplicationKafkaConsumer.Services
{
    public class CacheChannelConsumerService : BackgroundService
    {
        IChannelConsumer<OrderStreamModel> _consumer;
        ICache<OrderRequest> _cache;

        public CacheChannelConsumerService(
            IChannelConsumer<OrderStreamModel> consumer,
            ICache<OrderRequest> cache)
        {
            _consumer = consumer;
            _cache = cache;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                if (await _consumer.WaitToRead())
                {
                    var streamCat = await _consumer.Read();
                    lock (_cache)
                    {
                        ExecuteDicionaryCommand(streamCat);
                    }
                }
            }
        }

        private void ExecuteDicionaryCommand(OrderStreamModel streamCat)
        {
            // UPDATE (delete, insert)
            // DELETE (delete)
            // INSERT (insert)

            switch (streamCat.Command)
            {
                case ActionNames.Delete:
                    Console.WriteLine("Delete");
                    _cache.Delete(streamCat.OrderRequest.OrderId);
                    break;
                case ActionNames.Update:
                    Console.WriteLine("Update");
                    _cache.Delete(streamCat.OrderRequest.OrderId);
                    _cache.Set(streamCat.OrderRequest);
                    break;
                case ActionNames.Insert:
                    Console.WriteLine("Insert");
                    _cache.Set(streamCat.OrderRequest);
                    break;
            }
        }
    }
}
