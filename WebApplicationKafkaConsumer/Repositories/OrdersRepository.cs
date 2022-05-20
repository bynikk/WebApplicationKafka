using Microsoft.EntityFrameworkCore;
using WebApplicationKafkaConsumer.Database;
using WebApplicationKafkaConsumer.Entities;
using WebApplicationKafkaConsumer.Interfaces.Repositories;

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

        public virtual async Task Delete(int id)
        {
            var existingItem = await _context.Orderrequests.FirstOrDefaultAsync(x => x.OrderId == id);

            if (existingItem == null) throw new ArgumentNullException(nameof(existingItem));

            _context.Orderrequests.Remove(existingItem);
        }

        public Task<List<OrderRequest>> Get()
        {
            return _context.Orderrequests.ToListAsync();
        }

        public Task SaveChanges()
        {
            return _context.SaveChangesAsync();
        }

        public virtual Task Update(OrderRequest item)
        {
            _context.Entry(item).State = EntityState.Modified;
            return _context.SaveChangesAsync();
        }
    }
}
