using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebApplicationKafkaConsumer.Entities;
using WebApplicationKafkaConsumer.Interfaces;
using WebApplicationKafkaConsumer.Interfaces.Repositories;

namespace WebApplicationKafkaConsumer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderRequestController : ControllerBase
    {
        IRepository<OrderRequest> _repository;
        public OrderRequestController(IRepository<OrderRequest> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var orders = await _repository.Get();
            return Ok(JsonSerializer.Serialize(orders));
        }

        [HttpPost]
        public async Task<IActionResult> Post(OrderRequest model)
        {
            await _repository.Add(model);
            await _repository.SaveChanges();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put(OrderRequest model)
        {
            await _repository.Update(model);
            return Ok();
        }
    }
}
