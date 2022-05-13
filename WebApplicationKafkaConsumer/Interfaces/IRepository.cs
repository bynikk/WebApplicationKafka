namespace WebApplicationKafkaConsumer.Interfaces
{
    public interface IRepository<T> where T : class
    {
        public Task<T> Get();
        public Task Delete(int id);
        public Task Update(T item);
        public Task Add(T item);
    }
}
