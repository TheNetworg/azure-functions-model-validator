using AzureFunctionsValidator;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace FunctionApp1
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
            Request req,
            ILogger log)
        {
            return AzureFunctionFluentValidator<Request>.Init
                .OnSuccess(dto => new OkObjectResult($"Hello {dto.Name}"))
                .ValidateAndReturn(req);
        }
    }
}
