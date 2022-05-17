namespace WebApplicationKafkaConsumer.Interfaces.CacheDistribution.Producers
{
    public interface IChannelProducer<T> where T : class
    {
        public Task Write(T item);
    }
}
