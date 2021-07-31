using Application.Common.Exceptions;
using Application.Customer.Commands.DeleteCustomer;
using Domain.Aggregates.Customer;
using Domain.Aggregates.Customer.Strategies.Interfaces;
using Domain.Common;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Application.UnitTests.Customer.Commands.DeleteCustomer
{
    [TestFixture]
    public class DeleteCustomerCommandHandlerTests
    {
        [Test]
        public void Handle_RequestedCustomerNotExist_ThrowNotFoundEx()
        {
            //Arrange 
            var mockUow = new Mock<IUnitOfWork>();

            var mockCustomerRepo = new Mock<ICustomerRepository>();
            mockCustomerRepo.Setup(m => m.Find(It.IsAny<Expression<Func<Domain.Aggregates.Customer.Customer, bool>>>(), false)).Returns(new List<Domain.Aggregates.Customer.Customer>().AsQueryable());

            mockUow.Setup(m => m.Customers).Returns(mockCustomerRepo.Object);

            var deleteCustomerCommandHandlerObj = new DeleteCustomerCommandHandler(mockUow.Object);

            //Act & Assert
            Assert.ThrowsAsync<NotFoundException>(() => deleteCustomerCommandHandlerObj.Handle(new DeleteCustomerCommand { Id = Guid.NewGuid() }, new System.Threading.CancellationToken()));
        }

        [Test]
        public async Task Handle_RequestedCustomerExist_CommandResultCodeMustBeTrue()
        {
            //Arrange    
            var mockCustomerCreationPrerequisiteStrategy = new Mock<ICustomerCreationPrerequisiteStrategy>();

            var mockCustomer = Domain.Aggregates.Customer.Customer.Create("bla", "bla@bla", DateTime.Now, mockCustomerCreationPrerequisiteStrategy.Object);

            var mockCustomerRepo = new Mock<ICustomerRepository>();
            mockCustomerRepo.Setup(m => m.Find(It.IsAny<Expression<Func<Domain.Aggregates.Customer.Customer, bool>>>(), false))
                            .Returns(new List<Domain.Aggregates.Customer.Customer>() { mockCustomer }.AsQueryable());

            Mock<IUnitOfWork> mockUow = new Mock<IUnitOfWork>();
            mockUow.Setup(x => x.Customers).Returns(mockCustomerRepo.Object);

            var deleteCustomerCommandHandlerObj = new DeleteCustomerCommandHandler(mockUow.Object);

            var mockDeleteCustomerCommand = new Mock<DeleteCustomerCommand>();  
            mockDeleteCustomerCommand.SetupProperty(x => x.Id, mockCustomer.Id);

            //Act
            var result = await deleteCustomerCommandHandlerObj.Handle(mockDeleteCustomerCommand.Object, new System.Threading.CancellationToken());

            //Assert
            Assert.IsTrue(result.Succeeded);
        }
    }
}
