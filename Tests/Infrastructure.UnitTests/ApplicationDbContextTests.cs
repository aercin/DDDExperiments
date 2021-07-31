using Application.Common.Interfaces;
using Domain.Aggregates.Customer;
using Domain.Aggregates.Customer.Strategies.Interfaces;
using Domain.Common;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Infrastructure.UnitTests
{
    [TestFixture]
    public class ApplicationDbContextTests
    {
        ApplicationDbContext _dbContext;
        Mock<IDomainEventService> mockDomainEventService;

        [SetUp]
        public void Setup()
        {
            mockDomainEventService = new Mock<IDomainEventService>();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString("N")).Options;
            _dbContext = new ApplicationDbContext(options, mockDomainEventService.Object);
        }

        [TearDown]
        public void CleanUp()
        {
            _dbContext.Dispose();
        }

        [Test]
        public async Task DispatchEvents_WhenNewCustomerStoredToDB_VerifyDomainEventPublishMethodIsCalled()
        {
            //Arrange  
            var mockCustomerCreationPrerequisiteStrategy = new Mock<ICustomerCreationPrerequisiteStrategy>();
            var newCustomer = Customer.Create("blabla", "bla@bla", new DateTime(1986, 07, 29), mockCustomerCreationPrerequisiteStrategy.Object);

            _dbContext.Customers.Add(newCustomer);

            //Act
            var result = await _dbContext.SaveChangesAsync();

            //Assert 
            mockDomainEventService.Verify(m => m.Publish(It.IsAny<DomainEvent>()), Times.AtLeastOnce);
        }
    }
}
