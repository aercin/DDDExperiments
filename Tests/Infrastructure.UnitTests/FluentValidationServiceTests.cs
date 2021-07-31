using FluentValidation;
using FluentValidation.Results;
using Infrastructure.FluentValidation;
using Moq;
using NUnit.Framework;
using System;

namespace Infrastructure.UnitTests
{
    [TestFixture]
    public class FluentValidationServiceTests
    {
        [Test]
        public void Validate_ThereArentAnyValidationForType_IsValidPropertyShouldMarkTrue()
        {
            //Arrange
            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(x => x.GetService(typeof(IValidator<It.IsAnyType>))).Returns(null);

            var fluentValidationServiceObj = new FluentValidationService(mockServiceProvider.Object);

            //Act
            var result = fluentValidationServiceObj.Validate(new StubValidationClass());

            //Assert
            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public void Validate_ThereIsAnyValidationForType_VerifyValidateMethodIsCalled()
        {
            //Arrange 
            var mockValidator = new Mock<IValidator<StubValidationClass>>();
            mockValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<It.IsAnyType>>())).Returns(new ValidationResult());

            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(x => x.GetService(typeof(IValidator<StubValidationClass>))).Returns(mockValidator.Object);

            var fluentValidationServiceObj = new FluentValidationService(mockServiceProvider.Object);

            //Act
            var result = fluentValidationServiceObj.Validate(new StubValidationClass());

            //Assert
            mockValidator.Verify(x => x.Validate(It.IsAny<IValidationContext>()), Times.Once);
        }

        public class StubValidationClass
        {

        }

    }
}
