using System;
using Messages;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Microsoft.Extensions.Logging;

namespace NotificationService.Handlers
{
    public class CustomerCreatedHandler: IHandleMessages<CustomerCreated>
    {
        private readonly ILogger _logger;

        public  CustomerCreatedHandler(ILogger<CustomerCreatedHandler> logger): base()
        {
            _logger = logger;
        }
        
        
        public async Task Handle(CustomerCreated message, IMessageHandlerContext context)
        {
           _logger.LogInformation($"Processed Message: {message.Id} {message.FirstName} {message.LastName} {message.Age} {message.CommunicationPreference} {message.EmailAddress} {message.MobileNo}");
           await Task.CompletedTask;
        }
    }
} 