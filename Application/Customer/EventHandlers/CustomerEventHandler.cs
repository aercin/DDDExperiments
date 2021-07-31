using Application.Common.Models;
using Domain.Aggregates.Customer.Events;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Customer.EventHandlers
{
    public class CustomerEventHandler : INotificationHandler<DomainEventNotification<CustomerRegisteredEvent>>,
                                        INotificationHandler<DomainEventNotification<CustomerRemovedEvent>>,
                                        INotificationHandler<DomainEventNotification<CustomerUpdatedEvent>>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<CustomerEventHandler> _logger;

        public CustomerEventHandler(IHttpContextAccessor httpContextAccessor, ILogger<CustomerEventHandler> logger)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._logger = logger;
        }

        public Task Handle(DomainEventNotification<CustomerRegisteredEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            this._logger.LogInformation($"TrackId: {this._httpContextAccessor.HttpContext.Request.Headers["TrackId"]} CleanArchitecture Domain Event: {domainEvent.GetType().Name}");

            return Task.CompletedTask;
        }

        public Task Handle(DomainEventNotification<CustomerRemovedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            this._logger.LogInformation($"TrackId: {this._httpContextAccessor.HttpContext.Request.Headers["TrackId"]} CleanArchitecture Domain Event: {domainEvent.GetType().Name}");

            return Task.CompletedTask;
        }

        public Task Handle(DomainEventNotification<CustomerUpdatedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            this._logger.LogInformation($"TrackId: {this._httpContextAccessor.HttpContext.Request.Headers["TrackId"]} CleanArchitecture Domain Event: {domainEvent.GetType().Name}");

            return Task.CompletedTask;
        }
    }
}
