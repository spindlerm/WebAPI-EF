using System;
using NServiceBus;

namespace Messages
{
    public class CustomerDeleted : IMessage
    {
        public int Id { get; set; }
    };
}
