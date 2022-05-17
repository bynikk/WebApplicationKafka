using System.Threading.Channels;

namespace WebApplicationKafkaConsumer.Interfaces.CacheDistribution
{
    public interface IChannelContext<T> where T : class
    {
        public Channel<T> GetChannel();
    }
}
