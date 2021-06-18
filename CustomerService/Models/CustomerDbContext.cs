using System;
using System.Data;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using static webapi.Models.Customer;

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
                    Age = 24,
                    CommunicationPreference = CommunicationPreferences.Email,
                    MobileNo = "0776863451",
                    EmailAddress = "uncle.bob@test.com"
                }, new Customer
                {
                    Id = 2,
                    FirstName = "John",
                    LastName = "Smith",
                    Age = 55,
                    CommunicationPreference = CommunicationPreferences.SMS,
                    MobileNo = "0776565451",
                    EmailAddress = "John.Smith@test.com"
                }, new Customer
                {
                    Id = 3,
                    FirstName = "Phil",
                    LastName = "Colins",
                    Age = 55,
                    CommunicationPreference = CommunicationPreferences.None,
                    MobileNo = "0776864641",
                    EmailAddress = "phil.colins@test.com"
                });
        }

        public DbSet<Customer> Customers {get; set;}
        public DbSet<IntegrationEvent> IntegrationEventOutbox { get; set; }
    };
}
