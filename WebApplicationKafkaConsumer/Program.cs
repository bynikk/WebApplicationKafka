using Microsoft.EntityFrameworkCore;
using WebApplicationKafkaConsumer;
using WebApplicationKafkaConsumer.Database;
using WebApplicationKafkaConsumer.Entities;
using WebApplicationKafkaConsumer.Interfaces;
using WebApplicationKafkaConsumer.Repositories;
using WebApplicationKafkaConsumer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IHostedService, ApacheKafkaConsumerService>();
builder.Services.AddScoped<IRepository<OrderRequest>, OrdersRepository>();
builder.Services.AddScoped<kafkadbContext>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowOrigin", option => option.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddDbContext<kafkadbContext>(options =>
            options.UseNpgsql("Host=localhost;Port=5432;Database=kafkadb;Username=postgres;Password=123"));

var app = builder.Build();

app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
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
