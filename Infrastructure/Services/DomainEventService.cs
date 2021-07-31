using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Aggregates.StoredEvent;
using Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class DomainEventService : IDomainEventService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<DomainEventService> _logger;
        private readonly IPublisher _mediator;
        private readonly IServiceProvider _serviceProvider;

        public DomainEventService(IHttpContextAccessor httpContextAccessor, 
                                  ILogger<DomainEventService> logger, 
                                  IPublisher mediator,
                                  IServiceProvider serviceProvider)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._logger = logger;
            this._mediator = mediator;
            this._serviceProvider = serviceProvider;
        }

        public async Task Publish(DomainEvent domainEvent)
        {
            this._logger.LogInformation("TrackId:{trackId} Publishing domain event. Event - {event}", this._httpContextAccessor.HttpContext.Request.Headers["TrackId"], domainEvent.GetType().Name);

            if (!domainEvent.MessageType.Equals("DomainNotification"))
            {
                var serializedData = JsonConvert.SerializeObject(domainEvent);

                var storedEvent = new StoredEvent(domainEvent, serializedData);

                var existedUow = this._serviceProvider.GetRequiredService<IUnitOfWork>();
                existedUow.StoredEvents.Add(storedEvent);
            }

            await this._mediator.Publish(GetNotificationCorrespondingToDomainEvent(domainEvent));
        }

        private INotification GetNotificationCorrespondingToDomainEvent(DomainEvent domainEvent)
        {
            return (INotification)Activator.CreateInstance(typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType()), domainEvent);
        }
    }
}
