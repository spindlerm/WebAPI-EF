using System;
using NServiceBus;

namespace Messages
{
    public class CustomerDeleted : IEvent 
    {
        public int Id { get; set; }
    };
}
