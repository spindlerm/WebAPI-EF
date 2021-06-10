using System;
using NServiceBus;

namespace webapi.Events
{
    public class CustomerDeleted : IMessage
    {
        public int Id { get; set; }
    };
}
