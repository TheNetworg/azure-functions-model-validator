using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace AzureFunctionsValidator
{
    public class AzureFunctionFluentValidator<T> : AzureFunctionFluentValidatorBase<T>
    {
        private SuccessFunction _successFunction;
        private FailureFunction _failureFunction;

        public delegate IActionResult SuccessFunction(T dto);
        public delegate void FailureFunction(T dto, List<ValidationResult> results);

        public static AzureFunctionFluentValidator<T> Init => new AzureFunctionFluentValidator<T>();

        public IActionResult ValidateAndReturn(T dto)
        {
            if (_successFunction == null)
                throw new ArgumentNullException(nameof(SuccessFunction));

            var (isValid, results) = TryValidate(dto);

            if (isValid)
            {
                return _successFunction?.Invoke(dto);
            }
            else
            {
                _failureFunction?.Invoke(dto, results);
                return new BadRequestObjectResult(results);
            }
        }

        public AzureFunctionFluentValidator<T> OnSuccess(SuccessFunction successFunction)
        {
            _successFunction = successFunction;
            return this;
        }

        public AzureFunctionFluentValidator<T> OnFailure(FailureFunction failureFunction)
        {
            _failureFunction = failureFunction;
            return this;
        }
    }
}