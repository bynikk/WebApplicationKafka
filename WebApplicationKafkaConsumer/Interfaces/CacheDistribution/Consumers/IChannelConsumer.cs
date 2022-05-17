namespace WebApplicationKafkaConsumer.Interfaces.CacheDistribution.Producers
{
    public interface IChannelConsumer<T> where T : class
    {
        public Task<T> Read();
        public Task<bool> WaitToRead();
    }
}
