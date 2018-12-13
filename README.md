# Azure Functions model validator
Easy to use fluent API for validating modesl that arrived via HttpTrigger. Define what should happen when the model is ok. Optionally you can define on-failiure callback. The validator is also in Async version.

## Example

```cs
public class Request
    {
        [Required]
        public string Name { get; set; }
    }
    
    //...


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
        
        
```
