using System.Collections.Generic;

namespace Application.Common.Interfaces
{
    public interface IValidation
    {
        ValidationResult Validate<T>(T arg) where T : class;
    }

    public class ValidationResult
    {
        public bool IsValid { get; private set; }

        private List<string> _errors; 
        public IReadOnlyCollection<string> Errors => _errors?.AsReadOnly();

        public ValidationResult(bool isValid, List<string> errors)
        {
            this.IsValid = isValid;

            if(errors != null)
            {
                this._errors = new List<string>();
                this._errors.AddRange(errors);
            } 
        }
    }
}
