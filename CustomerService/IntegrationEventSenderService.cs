using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using webapi.Models;
using Newtonsoft.Json;
using NServiceBus;
using Messages;

namespace webapi
{
    public class IntegrationEventSenderService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<IntegrationEventSenderService> _logger;
        private readonly IMessageSession _messageSession;

        public IntegrationEventSenderService(IServiceScopeFactory scopeFactory, IMessageSession messageSession, ILogger<IntegrationEventSenderService> logger)
        {
            _scopeFactory = scopeFactory;
            _messageSession = messageSession;
            _logger = logger;
            using var scope = _scopeFactory.CreateScope();
            using var dbContext = scope.ServiceProvider.GetRequiredService<CustomerDbContext>();
            dbContext.Database.EnsureCreated();
        }

        protected  override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await PublishOutstandingIntegrationEvents(stoppingToken);
            }
        }

        private async Task PublishOutstandingIntegrationEvents(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    {
                        using var scope = _scopeFactory.CreateScope();
                        using var dbContext = scope.ServiceProvider.GetRequiredService<CustomerDbContext>();
                        var events = dbContext.IntegrationEventOutbox.OrderBy(o => o.ID).ToList();
                        foreach (var e in events)
                        {
                           switch(e.Event)
                           {
                                case "user.create":
                                {
                                    CustomerCreated integEvntData = JsonConvert.DeserializeObject<CustomerCreated>(e.Data);
                                    await _messageSession.Send("Server", integEvntData).ConfigureAwait(false);
                                    break;
                                }
                                case "user.delete":
                                {
                                    CustomerDeleted integEvntData = JsonConvert.DeserializeObject<CustomerDeleted>(e.Data);
                                    await _messageSession.Send("Server", integEvntData).ConfigureAwait(false);
                                    break;
                                }
                                default:
                                        Console.WriteLine("Default case");
                                        break;
                                }

                             string Message = $"Outbox Integration event published  {DateTime.UtcNow.ToLongTimeString()} {e.Event} {e.Data}";
                            _logger.LogInformation(Message);
                            dbContext.Remove(e);
                            dbContext.SaveChanges();
                        }
                    }
                    await Task.Delay(1000, stoppingToken);
                }     
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
