using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AzureFunctionsValidator
{
    public abstract class AzureFunctionFluentValidatorBase<T>
    {
        protected (bool, List<ValidationResult>) TryValidate(T dto)
        {
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, new ValidationContext(dto, null, null), results, true);
            return (isValid, results);
        }
    }
}
