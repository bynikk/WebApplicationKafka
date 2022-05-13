using WebApplicationKafkaConsumer.Database;
using WebApplicationKafkaConsumer.Entities;
using WebApplicationKafkaConsumer.Interfaces;

namespace WebApplicationKafkaConsumer.Repositories
{
    public class OrdersRepository : IRepository<OrderProcessingRequest>
    {
        kafkadbContext _context;
        public OrdersRepository(kafkadbContext context)
        {
            _context = context;
        }
        public Task Add(OrderProcessingRequest item)
        {
            _context.Orderrequests.Add(item);
            return _context.SaveChangesAsync();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<OrderProcessingRequest> Get()
        {
            throw new NotImplementedException();
        }

        public Task Update(OrderProcessingRequest item)
        {
            throw new NotImplementedException();
        }
    }
}
