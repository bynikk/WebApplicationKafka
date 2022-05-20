using Microsoft.EntityFrameworkCore;
using WebApplicationKafkaConsumer.CacheDistribution;
using WebApplicationKafkaConsumer.CacheDistribution.Consumers;
using WebApplicationKafkaConsumer.CacheDistribution.Producers;
using WebApplicationKafkaConsumer.Database;
using WebApplicationKafkaConsumer.Entities;
using WebApplicationKafkaConsumer.Interfaces;
using WebApplicationKafkaConsumer.Interfaces.CacheDistribution;
using WebApplicationKafkaConsumer.Interfaces.CacheDistribution.Producers;
using WebApplicationKafkaConsumer.Interfaces.Repositories;
using WebApplicationKafkaConsumer.Repositories;
using WebApplicationKafkaConsumer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddHostedService<ApacheKafkaConsumerService>();
builder.Services.AddHostedService<NotifyPostgresService>();
builder.Services.AddHostedService<CacheChannelConsumerService>();
builder.Services.AddScoped<IRepository<OrderRequest>, OrdersRepositoryCache>();
builder.Services.AddScoped<kafkadbContext>();

//
builder.Services.AddSingleton<IChannelConsumer<OrderStreamModel>, OrdersChannelConsumer>();
builder.Services.AddSingleton<IChannelProducer<OrderStreamModel>, OrdersChannelProducer>();

builder.Services.AddSingleton<ICache<OrderRequest>, OrdersCache>();
builder.Services.AddSingleton<IChannelContext<OrderStreamModel>, OrdersChannelContext>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<kafkadbContext>(options =>
            options.UseNpgsql("Host=localhost;Port=5432;Database=kafkadb;Username=postgres;Password=123"));

var app = builder.Build();

// Configure the HTTP request pipeline. 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
