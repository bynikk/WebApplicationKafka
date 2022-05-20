using System.Threading.Channels;
using WebApplicationKafkaConsumer.Interfaces.CacheDistribution;
using WebApplicationKafkaConsumer.Interfaces.CacheDistribution.Producers;

namespace WebApplicationKafkaConsumer.CacheDistribution.Consumers
{
    public class OrdersChannelConsumer : IChannelConsumer<OrderStreamModel>
    {
        Channel<OrderStreamModel> channel;

        public OrdersChannelConsumer(IChannelContext<OrderStreamModel> channelContext)
        {
            channel = channelContext.GetChannel();
        }

        public Task<OrderStreamModel> Read()
        {
            return channel.Reader.ReadAsync().AsTask();
        }

        public Task<bool> WaitToRead()
        {
            return channel.Reader.WaitToReadAsync().AsTask();
        }
    }
}
