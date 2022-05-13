namespace WebApplicationKafkaConsumer.Entities
{
    public class OrderProcessingRequest
    {
        public int OrderId { get; set; }
        public string? Value { get; set; }
    }
}
