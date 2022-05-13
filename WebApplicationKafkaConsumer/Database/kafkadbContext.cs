using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using WebApplicationKafkaConsumer.Entities;

namespace WebApplicationKafkaConsumer.Database
{
    public partial class kafkadbContext : DbContext
    {
        public kafkadbContext()
        {
        }

        public kafkadbContext(DbContextOptions<kafkadbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<OrderProcessingRequest> Orderrequests { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=kafkadb;Username=postgres;Password=123");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderProcessingRequest>().HasKey(j => j.OrderId);
            modelBuilder.Entity<OrderProcessingRequest>(entity =>
            {
                entity.ToTable("orderrequest");

                entity.Property(e => e.OrderId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("orderid");

                entity.Property(e => e.Value)
                    .HasMaxLength(500)
                    .HasColumnName("value");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
