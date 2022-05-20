using WebApplicationKafkaConsumer.Entities;

namespace WebApplicationKafkaConsumer.CacheDistribution
{
    public class OrderStreamModel
    {
        public OrderStreamModel(string command, OrderRequest order)
        {
            Command = command;
            OrderRequest = order;
        }

        public string Command;

        public OrderRequest OrderRequest;
    }
}
