using Application.Common.Models;
using Application.Customer.EventHandlers;
using Domain.Aggregates.Customer.Events;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Application.UnitTests.Customer.EventHandlers
{
    [TestFixture]
    public class CustomerEventHandlerTests
    {
        [Test]
        public async Task Handle_IsInformationLogCalledAtOnceForCustomerRegisterEvent_VerifyCallingOfLogInformationMethod()
        {
            //Arrange
     
            var dummyHttpContext = new DefaultHttpContext();
            dummyHttpContext.Request.Headers["TrackId"] = "dummyTrackId";
     
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            mockHttpContextAccessor.Setup(m => m.HttpContext).Returns(dummyHttpContext);

            var mockLogger = new Mock<ILogger<CustomerEventHandler>>();

            var mockNotification = new Mock<DomainEventNotification<CustomerRegisteredEvent>>(Mock.Of<CustomerRegisteredEvent>());

            var customerEventHandlerObj = new CustomerEventHandler(mockHttpContextAccessor.Object, mockLogger.Object);

            //Act
            await customerEventHandlerObj.Handle(mockNotification.Object, new System.Threading.CancellationToken());

            //Assert
            mockLogger.Verify(m => m.Log(
                It.Is<LogLevel>(x => x == LogLevel.Information),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Once);
        }
    }
}
