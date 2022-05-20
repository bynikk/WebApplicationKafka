using System.Threading.Channels;
using WebApplicationKafkaConsumer.Interfaces.CacheDistribution;

namespace WebApplicationKafkaConsumer.CacheDistribution
{
    public class OrdersChannelContext : IChannelContext<OrderStreamModel>
    {
        Channel<OrderStreamModel> channel;

        public OrdersChannelContext()
        {
            channel = Channel.CreateUnbounded<OrderStreamModel>();        
        }

        public Channel<OrderStreamModel> GetChannel()
        {
            return channel;
        }
    }
}
