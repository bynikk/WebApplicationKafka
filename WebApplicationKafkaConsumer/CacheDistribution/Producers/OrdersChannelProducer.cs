using System.Threading.Channels;
using WebApplicationKafkaConsumer.Interfaces.CacheDistribution;
using WebApplicationKafkaConsumer.Interfaces.CacheDistribution.Producers;

namespace WebApplicationKafkaConsumer.CacheDistribution.Producers
{
    public class OrdersChannelProducer : IChannelProducer<OrderStreamModel>
    {
        Channel<OrderStreamModel> channel;

        public OrdersChannelProducer(IChannelContext<OrderStreamModel> channelContext)
        {
            channel = channelContext.GetChannel();
        }

        public Task Write(OrderStreamModel item)
        {
            return channel.Writer.WriteAsync(item).AsTask();
        }
    }
}
