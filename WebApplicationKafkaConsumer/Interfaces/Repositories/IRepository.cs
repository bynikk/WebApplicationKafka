
namespace WebApplicationKafkaConsumer.Interfaces.Repositories
{
    public interface IRepository<T> where T : class
    {
        public Task<List<T>> Get();
        public Task Delete(int id);
        public Task Update(T item);
        public Task Add(T item);
        public Task SaveChanges();
    }
}
