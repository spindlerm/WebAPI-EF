using System;
using System.Data;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;

namespace webapi.Models
{
    public class CustomerDbContext :DbContext
    {

        public CustomerDbContext(DbContextOptions options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    Id = 1,
                    FirstName = "Uncle",
                    LastName = "Bob",
                    Age = 24
                }, new Customer
                {
                    Id = 2,
                    FirstName = "John",
                    LastName = "Smith",
                    Age = 55
                });
        }

        public DbSet<Customer> Customers {get; set;}


    };
}
