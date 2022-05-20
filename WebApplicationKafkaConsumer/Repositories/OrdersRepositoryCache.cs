using Microsoft.EntityFrameworkCore;
using WebApplicationKafkaConsumer.Database;
using WebApplicationKafkaConsumer.Entities;
using WebApplicationKafkaConsumer.Interfaces.CacheDistribution;

namespace WebApplicationKafkaConsumer.Repositories
{
    public class OrdersRepositoryCache : OrdersRepository
    {
        ICache<OrderRequest> _cache;
        kafkadbContext _context;

        public OrdersRepositoryCache(
            kafkadbContext context,
            ICache<OrderRequest> cache) : base(context)
        {
            _context = context;
            _cache = cache;
            _context.ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public override Task Delete(int id)
        {
            _cache.Delete(id);

            return base.Delete(id);
        }

        public override Task Update(OrderRequest item)
        {
            int cacheKey = item.OrderId;
            OrderRequest? order = _cache.Get(cacheKey);

            if (order != null)
            {
                Console.WriteLine("FromCache!!");
            }
            if (order == null)
            {
                order = _context.Orderrequests.AsNoTracking().FirstOrDefault(o => o.OrderId == item.OrderId);
            }

            if (order == null) throw new ArgumentNullException("Database don't exists such item.");

            _cache.Delete(cacheKey);
            _cache.Set(item);

            return base.Update(item);
        }
    }
}
