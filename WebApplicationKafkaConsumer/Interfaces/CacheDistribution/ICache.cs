namespace WebApplicationKafkaConsumer.Interfaces.CacheDistribution
{
    public interface ICache<T> where T : class
    {
        public T Get(int key);
        public void Set(T item);
    }
}
