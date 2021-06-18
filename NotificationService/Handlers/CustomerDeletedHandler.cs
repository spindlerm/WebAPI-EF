using System;
using Messages;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Microsoft.Extensions.Logging;


namespace NotificationService.Handlers
{
    public class CustomerDeletedHandler: IHandleMessages<CustomerDeleted>
    {
        private readonly ILogger _logger;

        public  CustomerDeletedHandler(ILogger<CustomerDeletedHandler> logger): base()
        {
            _logger = logger;
        }
        
        
        public async Task Handle(CustomerDeleted message, IMessageHandlerContext context)
        {
           _logger.LogInformation($"Processed Message: {message.Id}");

           await Task.CompletedTask;
        }
    }
}
