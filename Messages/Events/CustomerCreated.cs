using System;
using NServiceBus;

namespace Messages
{
    public class CustomerCreated : IMessage
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
    };
}