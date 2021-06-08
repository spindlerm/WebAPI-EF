using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using webapi.Models;
using NServiceBus;



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
        }

        [HttpGet]
        public IActionResult Get()
        {
            IEnumerable<Customer> customers = _context.Customers.ToList();
            return Ok(customers);
        }

        
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
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
            if (customer == null)
            {
                return BadRequest("Customer is null.");
            }
            
            _context.Add(customer);
            _context.SaveChanges();

            return CreatedAtRoute(
                  "Get", 
                  new { Id = customer.Id },
                  customer);
        }

        // DELETE: api/Customer/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Customer customer = _context.Customers.Find(id);
            if (customer == null)
            {
                return NotFound("The Customer record couldn't be found.");
            }
            _context.Customers.Remove(customer);
            _context.SaveChanges();
            return NoContent();
        }
    };
}
