namespace MyDishesAPI.EndpointFilters;

public class LogNotFoundResponseFilter : IEndpointFilter
{
    private readonly ILogger<LogNotFoundResponseFilter> _logger;

    public LogNotFoundResponseFilter(ILogger<LogNotFoundResponseFilter> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        // Invoke the next filter in the pipeline
        var result = await next(context);

        var actualResult = (result is INestedHttpResult) ? ((INestedHttpResult)result).Result
            : (IResult)result;

        if ((actualResult as IStatusCodeHttpResult)?.StatusCode == (int)StatusCodes.Status404NotFound)
        {
            // Log the 404 Not Found response
            _logger.LogInformation($"Resource not found: {context.HttpContext.Request.Path}");
        }

        return result;
    }
}
