using System.Threading.Tasks;
using AzureFunctionsValidator;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace FunctionApp1
{
    public static class TestingFunction
    {
        [FunctionName(nameof(Sync))]
        public static IActionResult Sync(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
            Request req,
            ILogger log)
        {
            return AzureFunctionFluentValidator<Request>.Init
                .OnSuccess(dto => new OkObjectResult($"Hello {dto.Name}"))
                .OnFailure((dto, results) =>
                {
                    results.ForEach(x => log.LogWarning(x.ErrorMessage));
                })
                .ValidateAndReturn(req);
        }

        [FunctionName(nameof(Async))]
        public static Task<IActionResult> Async(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
            Request req,
            ILogger log)
        {
            return AzureFunctionFluentAsyncValidator<Request>.Init
                .OnSuccess(async dto =>
                {
                    log.LogInformation($"Waiting for one second");
                    await Task.Delay(1000);
                    return new OkObjectResult($"Hello {dto.Name}");
                })
                .OnFailure(async (dto, results) =>
                {
                    log.LogInformation($"Waiting for one second");
                    await Task.Delay(1000);
                    results.ForEach(x => log.LogWarning(x.ErrorMessage));
                })
                .ValidateAndReturnAsync(req);
        }
    }
}
