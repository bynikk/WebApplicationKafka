using Microsoft.EntityFrameworkCore;
using WebApplicationKafkaConsumer.Entities;

namespace WebApplicationKafkaConsumer.Database
{
    public class ApplicationContext : DbContext
    {
        DbSet<OrderProcessingRequest> Requests;

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

    }
}
