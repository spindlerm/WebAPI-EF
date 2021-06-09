using System;
using NServiceBus;

namespace webapi.Events
{
    public class CustomerCreated : IMessage
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
    };
}
