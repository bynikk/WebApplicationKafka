using WebApplicationKafkaConsumer.Entities;
using WebApplicationKafkaConsumer.Interfaces.CacheDistribution;

namespace WebApplicationKafkaConsumer.CacheDistribution
{
    public class OrdersCache : ICache<OrderRequest>
    {
        private Dictionary<int, WeakReference> Orders = new();

        public OrderRequest? Get(int key)
        {
            if (Orders.Keys.Contains(key) && Orders[key].IsAlive)
            {
                return Orders[key].Target as OrderRequest;
            }

            return null;
        }

        public void Set(OrderRequest item)
        {
            Orders.Add(item.OrderId, new WeakReference(item));
        }

        public void Delete(int key)
        {
            Orders.Remove(key);
        }
    }
}
