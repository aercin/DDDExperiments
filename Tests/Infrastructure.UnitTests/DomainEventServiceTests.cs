using Application.Common.Interfaces;
using Domain.Common;
using Infrastructure.Persistence;
using Infrastructure.Persistence.UnitOfWork;
using Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using System.Linq;
using Domain.Aggregates.StoredEvent;

namespace Infrastructure.UnitTests
{
    [TestFixture]
    public class DomainEventServiceTests
    {
        Mock<ILogger<DomainEventService>> mockLogger;
        Mock<IHttpContextAccessor> mockHttpContextAccessor;
        Mock<IPublisher> mockPublisher;
        Mock<IServiceProvider> mockServiceProvider;
        DomainEventService _domainEventServiceObj;

        [SetUp]
        public void Setup()
        {
            //Arrange
            mockLogger = new Mock<ILogger<DomainEventService>>();

            mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            mockHttpContextAccessor.Setup(m => m.HttpContext).Returns(new DefaultHttpContext());

            mockPublisher = new Mock<IPublisher>();

            var mockUow = new Mock<IUnitOfWork>();

            var mockStoredEventRepository = new Mock<IStoredEventRepository>();
            mockUow.Setup(x => x.StoredEvents).Returns(mockStoredEventRepository.Object);

            mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(x => x.GetService(typeof(IUnitOfWork))).Returns(mockUow.Object);

            _domainEventServiceObj = new DomainEventService(mockHttpContextAccessor.Object, mockLogger.Object, mockPublisher.Object, mockServiceProvider.Object);
        }

        [Test]
        public async Task Publish_WhenDomainEventMessageTypeDifferentFromDomainNotification_StoredEventInstanceIsAdded()
        {
            //Act 
            await _domainEventServiceObj.Publish(new StubDomainEvent(Guid.NewGuid()));

            //Assert 
            mockServiceProvider.Verify(x => x.GetService(typeof(IUnitOfWork)), Times.Once);
        }

        [Test]
        public async Task Publish_IsMediatorCallPublishMethod_VerifyPublishMethodIsCalled()
        {
            //Act 
            await _domainEventServiceObj.Publish(new StubDomainEvent(Guid.NewGuid()));

            //Assert 
            mockPublisher.Verify(x => x.Publish(It.IsAny<INotification>(), new System.Threading.CancellationToken()), Times.Once);
        }


        public class StubDomainEvent : DomainEvent
        {
            public StubDomainEvent(Guid aggregateId) : base(aggregateId)
            {

            }
        }
    }
}
