using Application.Customer.Commands.CreateCustomer;
using Domain.Aggregates.Customer;
using Domain.Aggregates.Customer.Strategies.Interfaces;
using Domain.Common;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Application.UnitTests.Customer.Commands.CreateCustomer
{
    [TestFixture]
    public class CreateCustomerCommandHandlerTests
    {
        [Test]
        public async Task Handle_CreationOfCustomerSuccessfully_ResultCodeMustBeTrue()
        {
            //Arrange
            var mockUow = new Mock<IUnitOfWork>();
            var mockCustomerRepo = new Mock<ICustomerRepository>();
            mockUow.Setup(x => x.Customers).Returns(mockCustomerRepo.Object);

            var mockCustomerCreationPrerequisiteStrategy = new Mock<ICustomerCreationPrerequisiteStrategy>();

            var mockCreateCustomerCommand = new Mock<CreateCustomerCommand>();

            var createCustomerCommandHandlerObj = new CreateCustomerCommandHandler(mockUow.Object, mockCustomerCreationPrerequisiteStrategy.Object);

            //Act
            var result = await createCustomerCommandHandlerObj.Handle(mockCreateCustomerCommand.Object, new System.Threading.CancellationToken());

            //Assert
            Assert.IsTrue(result.Succeeded);
        }
    }
}
