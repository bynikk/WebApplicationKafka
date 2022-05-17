using System.Threading.Channels;
using WebApplicationKafkaConsumer.Interfaces.CacheDistribution;

namespace WebApplicationKafkaConsumer.CacheDistribution
{
    public class OrdersChannelContext : IChannelContext<string>
    {
        Channel<string> channel;

        public OrdersChannelContext()
        {
            channel = Channel.CreateUnbounded<string>();        
        }

        public Channel<string> GetChannel()
        {
            return channel;
        }
    }
}
