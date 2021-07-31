using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Behaviours
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : class, IRequest<TResponse>
                                                                                                   where TResponse : Result
    { 
        private readonly IValidation _validator; 

        public ValidationBehaviour(IValidation validator)
        {
            this._validator = validator;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var validationResultObj = this._validator.Validate<TRequest>(request);
            
            if (!validationResultObj.IsValid)
            { 
                throw new System.Exception($"Validation error is occured. Details: {JsonConvert.SerializeObject(validationResultObj.Errors)}");
            }

            return await next();
        }
    }
}
