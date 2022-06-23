using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entitites;
using API.Entitites.WBEntities;
using API.Entitites.PrintEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
     
        protected override async void OnModelCreating(ModelBuilder modelBuilder)
        {
            //++DateTime To UTC Value Converter
            //Конвертируем все значения модель DateTime в UTC, так как postgres не принимает других
            var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
                v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            var nullableDateTimeConverter = new ValueConverter<DateTime?, DateTime?>(
                v => v.HasValue ? v.Value.ToUniversalTime() : v,
                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (entityType.IsKeyless)
                {
                    continue;
                }

                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime))
                    {
                        property.SetValueConverter(dateTimeConverter);
                    }
                    else if (property.ClrType == typeof(DateTime?))
                    {
                        property.SetValueConverter(nullableDateTimeConverter);
                    }
                }
            }
            //--DateTime To UTC Value Converter
        }
      
        public DbSet<AppUser>? Users { get; set;}
        public DbSet<Income>? Incomes { get; set;}
        public DbSet<IncomeDetail>? IncomeDetails { get; set;}
        public DbSet<Order>? Orders { get; set;}
        public DbSet<OrderDetail>? OrderDetails { get; set;}
        public DbSet<Sale>? Sales { get; set;}

        public DbSet<PrintOrder>? PrintOrders { get; set;}
        public DbSet<FBSProduct>? FBS { get; set;}
        public DbSet<FBOProduct>? FBO { get; set;}
    }
}