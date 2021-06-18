using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using webapi.Models;
using Newtonsoft.Json;


namespace webapi.Controllers
{
    [Produces("application/json")]
    [Route("api/customers")]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly CustomerDbContext _context;

        public CustomerController(CustomerDbContext context,  ILogger<CustomerController> logger)
        {
            _context = context;
            _logger = logger;

        }

        [HttpGet]
        public IActionResult Get()
        {
             _logger.LogInformation($"Get ALL customers request");

            IEnumerable<Customer> customers = _context.Customers.ToList();
            return Ok(customers);
        }

        
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
            _logger.LogInformation($"Get customer: {id} request");

            Customer customer = _context.Customers.Find(id);
            if (customer == null)
            {
                return NotFound("The Customer record couldn't be found.");
            }
            
            return Ok(customer);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Customer customer)
        {
            _logger.LogInformation($"Posting new customer request");

            if (customer == null)
            {
                return BadRequest("Customer is null.");
            }

            using(var transaction = _context.Database.BeginTransaction())
            {
                try
                    {
                        _context.Add(customer);
                        _context.SaveChanges();

                        var integrationEventData = JsonConvert.SerializeObject(new
                        {
                            id = customer.Id,
                            firstName = customer.FirstName,
                            lastName = customer.LastName,
                            age = customer.Age
                        });



                        _context.IntegrationEventOutbox.Add(
                        new IntegrationEvent()
                        {
                            Event = "user.create",
                            Data = integrationEventData
                        });

                        _context.SaveChanges();
                        transaction.Commit();
                    }
                catch(Exception)  
                {
                    transaction.Rollback();
                }
            }

            return CreatedAtRoute(
                  "Get", 
                  new { Id = customer.Id },
                  customer);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
             _logger.LogInformation($"Delete customer: {id} request");

            Customer customer = _context.Customers.Find(id);
            if (customer == null)
            {
                return NotFound("The Customer record couldn't be found.");
            }
            using(var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Remove(customer);
                    _context.SaveChanges();

                    var integrationEventData = JsonConvert.SerializeObject(new
                    {
                        id = customer.Id,
                    });

                    _context.IntegrationEventOutbox.Add(
                    new IntegrationEvent()
                    {
                        Event = "user.delete",
                        Data = integrationEventData
                    });

                    _context.SaveChanges();
                    transaction.Commit();             
                }
                catch(Exception)
                {
                    transaction.Rollback();
                }
            }
        
            return NoContent();
        }
    };
}
