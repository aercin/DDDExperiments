using Application.Common.Interfaces;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Infrastructure.FluentValidation
{
    public class FluentValidationService : IValidation
    {
        private readonly IServiceProvider _serviceProvider;

        public FluentValidationService(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        public ValidationResult Validate<T>(T arg) where T : class
        {
            var context = new ValidationContext<T>(arg);

            ValidationResult validationResultObj = null;

            var validatorInstance = this._serviceProvider.GetService<IValidator<T>>();

            if (validatorInstance != null)
            {
                var fluentValidationResult = validatorInstance.Validate(context);

                validationResultObj = new ValidationResult(fluentValidationResult.IsValid, fluentValidationResult.Errors.Select(x => x.ErrorMessage).ToList());
            }
            else
            {
                validationResultObj = new ValidationResult(true, null);
            }

            return validationResultObj;
        }
    }
}
