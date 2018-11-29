using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AzureFunctionsValidator
{
    public class AzureFunctionFluentAsyncValidator<T> : AzureFunctionFluentValidatorBase<T>
    {
        private SuccessTask _successTask;
        private FailureTask _failureTask;
        public delegate Task<IActionResult> SuccessTask(T dto);
        public delegate Task FailureTask(T dto, List<ValidationResult> results);

        public static AzureFunctionFluentAsyncValidator<T> Init => new AzureFunctionFluentAsyncValidator<T>();

        public AzureFunctionFluentAsyncValidator<T> OnSuccess(SuccessTask successTask)
        {
            _successTask = successTask;
            return this;
        }

        public AzureFunctionFluentAsyncValidator<T> OnFailure(FailureTask failureTask)
        {
            _failureTask = failureTask;
            return this;
        }

        public async Task<IActionResult> ValidateAndReturnAsync(T dto)
        {
            if (_successTask == null)
                throw new ArgumentNullException(nameof(SuccessTask));

            var (isValid, results) = TryValidate(dto);
            if (isValid)
            {
                return await _successTask(dto);
            }
            else
            {
                if (_failureTask != null)
                    await _failureTask(dto, results);
                return new BadRequestObjectResult(results);
            }
        }
    }
}