using Application.Common.Behaviours;
using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Application.UnitTests.Common.Behaviours
{
    [TestFixture]
    public class ValidationBehaviourTests
    {
        Mock<IValidation> mockValidation;
        Mock<RequestHandlerDelegate<Result>> mockRequestHandlerDelegate;
        ValidationBehaviour<IRequest<Result>, Result> validationBehaviourObj;

        [SetUp]
        public void Setup()
        {
            //Arrange
            mockValidation = new Mock<IValidation>(); 

            mockRequestHandlerDelegate = new Mock<RequestHandlerDelegate<Result>>();

            validationBehaviourObj = new ValidationBehaviour<IRequest<Result>, Result>(mockValidation.Object);
        }

        [Test]
        public async Task Handle_WhenValidationIsValid_DelegateMethodIsCalled()
        {
            //Arrange
            mockValidation.Setup(x => x.Validate(It.IsAny<IRequest<Result>>())).Returns(new ValidationResult(true, null));

            //Act
            await validationBehaviourObj.Handle(null, new System.Threading.CancellationToken(), mockRequestHandlerDelegate.Object);

            //Assert
            mockRequestHandlerDelegate.Verify(m => m(), Times.Once);
        }

        [Test]
        public void Handle_WhenValidationIsInValid_ThrowException()
        {
            //Arrange
            mockValidation.Setup(x => x.Validate(It.IsAny<IRequest<Result>>())).Returns(new ValidationResult(false, null));

            //Act & Assert
            Assert.ThrowsAsync<Exception>(() => validationBehaviourObj.Handle(null, new System.Threading.CancellationToken(), mockRequestHandlerDelegate.Object));
        } 
    }
}
