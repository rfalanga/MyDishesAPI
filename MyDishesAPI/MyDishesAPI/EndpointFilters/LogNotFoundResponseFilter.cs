namespace MyDishesAPI.EndpointFilters;

public class LogNotFoundResponseFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        // Invoke the next filter in the pipeline
        var result = await next(context);
        // Check if the result is a NotFound result
        if (result is not null && context.HttpContext.Response.StatusCode == StatusCodes.Status404NotFound)
        {
            // Log the not found response
            Console.WriteLine($"Resource not found: {context.HttpContext.Request.Path}");
        }
        return result;
    }
}
