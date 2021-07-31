using Application.Common.Interfaces;
using Domain.Aggregates.Customer;
using Domain.Aggregates.Customer.Strategies.Interfaces;
using Domain.Common;
using Infrastructure.Persistence;
using Infrastructure.Persistence.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace Infrastructure.UnitTests
{
    [TestFixture]
    public class UnitOfWorkTests
    {
        IUnitOfWork _uow;
        ApplicationDbContext _dbContext;
        Mock<IDomainEventService> mockDomainEventService;

        [SetUp]
        public void Setup()
        {
            mockDomainEventService = new Mock<IDomainEventService>();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString("N")).Options;
            _dbContext = new ApplicationDbContext(options, mockDomainEventService.Object);

            _uow = new UnitOfWork(_dbContext);
        }

        [TearDown]
        public void CleanUp()
        {
            _uow.Dispose();
        }

        [Test]
        public async Task UnitOfWork_AddingNewCustomer_ReturnTrue()
        {

            var mockCustomerCreationPrerequisiteStrategy = new Mock<ICustomerCreationPrerequisiteStrategy>();
            var newCustomer = Customer.Create("blabla", "bla@bla", new DateTime(1986, 07, 29), mockCustomerCreationPrerequisiteStrategy.Object);

            this._uow.Customers.Add(newCustomer);
            await this._uow.CompleteAsync(new System.Threading.CancellationToken());

            Assert.IsTrue(this._uow.Customers.Find(x => x.Name == "blabla" && x.Email == "bla@bla", true).Any());
        }

        [Test]
        public async Task UnitOfWork_UpdatingExistedCustomer_ReturnTrue()
        {
            var mockCustomerCreationPrerequisiteStrategy = new Mock<ICustomerCreationPrerequisiteStrategy>();
            var newCustomer = Customer.Create("blabla", "bla@bla", new DateTime(1986, 07, 29), mockCustomerCreationPrerequisiteStrategy.Object);

            this._uow.Customers.Add(newCustomer);
            await this._uow.CompleteAsync(new System.Threading.CancellationToken());

            var existedCustomer = this._uow.Customers.Find(x => x.Name == "blabla" && x.Email == "bla@bla", false).Single();
            var mockCustomerModificationPrerequisiteStrategy = new Mock<ICustomerModificationPrerequisiteStrategy>();

            existedCustomer.Update("blabla1", existedCustomer.Email, existedCustomer.BirthDate, mockCustomerModificationPrerequisiteStrategy.Object);
            await this._uow.CompleteAsync(new System.Threading.CancellationToken());

            Assert.IsTrue(this._uow.Customers.Find(x => x.Name == "blabla1", true).Any());
        }

        [Test]
        public async Task UnitOfWork_RemovingExistedCustomer_ReturnTrue()
        {
            var mockCustomerCreationPrerequisiteStrategy = new Mock<ICustomerCreationPrerequisiteStrategy>();
            var newCustomer = Customer.Create("blabla", "bla@bla", new DateTime(1986, 07, 29), mockCustomerCreationPrerequisiteStrategy.Object);

            this._uow.Customers.Add(newCustomer);
            await this._uow.CompleteAsync(new System.Threading.CancellationToken());

            var existedCustomer = this._uow.Customers.Find(x => x.Name == "blabla" && x.Email == "bla@bla", false).Single();
            this._uow.Customers.Remove(existedCustomer);
            await this._uow.CompleteAsync(new System.Threading.CancellationToken());

            Assert.IsTrue(this._uow.Customers.Find(x => x.Name == "blabla", true).Count() == 0);
        }

        [Test]
        public async Task UnitOfWork_IsFetchAllCustomer_ReturnTrue()
        {
            var mockCustomerCreationPrerequisiteStrategy = new Mock<ICustomerCreationPrerequisiteStrategy>();
            var newCustomer1 = Customer.Create("blabla1", "bla1@bla1", new DateTime(1986, 07, 29), mockCustomerCreationPrerequisiteStrategy.Object);
            this._uow.Customers.Add(newCustomer1);
            var newCustomer2 = Customer.Create("blabla2", "bla2@bla2", new DateTime(1986, 07, 29), mockCustomerCreationPrerequisiteStrategy.Object);
            this._uow.Customers.Add(newCustomer2);

            await _uow.CompleteAsync(new System.Threading.CancellationToken());

            var result = _uow.Customers.All(isNoTracking: true);

            Assert.IsTrue(result != null);
            Assert.IsTrue(result.Count() == 2);
            Assert.IsTrue(result.First().Name == "blabla1");
            Assert.IsTrue(result.Last().Name == "blabla2");
        } 
    }
}
