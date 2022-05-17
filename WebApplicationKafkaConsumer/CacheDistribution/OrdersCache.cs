using WebApplicationKafkaConsumer.Entities;
using WebApplicationKafkaConsumer.Interfaces.CacheDistribution;

namespace WebApplicationKafkaConsumer.CacheDistribution
{
    public class OrdersCache : ICache<OrderRequest>
    {
        private Dictionary<int, OrderRequest> _orders = new Dictionary<int, OrderRequest>();

        public OrderRequest Get(int key)
        {
            var existingOrder = _orders.FirstOrDefault(o => o.Key == key); ;

            return existingOrder.Value;
        }

        public void Set(OrderRequest item)
        {
            _orders.Add(item.OrderId, item);
        }
    }
}
