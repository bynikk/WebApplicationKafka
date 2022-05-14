using WebApplicationKafkaConsumer.Database;
using WebApplicationKafkaConsumer.Entities;
using WebApplicationKafkaConsumer.Interfaces;

namespace WebApplicationKafkaConsumer.Repositories
{
    public class OrdersRepository : IRepository<OrderRequest>
    {
        kafkadbContext _context;
        public OrdersRepository(kafkadbContext context)
        {
            _context = context;
            _context.ChangeTracker.AutoDetectChangesEnabled = false;
        }
        public Task Add(OrderRequest item)
        {
            return _context.Orderrequests.AddAsync(item).AsTask();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<OrderRequest> Get()
        {
            throw new NotImplementedException();
        }

        public Task SaveChanges()
        {
            return _context.SaveChangesAsync();
        }

        public Task Update(OrderRequest item)
        {
            throw new NotImplementedException();
        }
    }
}
