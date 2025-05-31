namespace MyDishesAPI.EndpointFilters;

public class RendangDishIsLockedFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        Guid dishId;
        if (context.HttpContext.Request.Method == "Put")
        {
            dishId = context.GetArgument<Guid>(2);
        }
        else if (context.HttpContext.Request.Method == "Delete")
        {
            dishId = context.GetArgument<Guid>(1);
        }
        else
        {
            throw new NotSupportedException($"Method {context.HttpContext.Request.Method} is not supported by this filter.");
        }
        var rendangId = new Guid("b0c8f1d2-3e4f-5a6b-7c8d-9e0f1a2b3c4d"); // Example Rendang dish ID, not the same value as Kevin's code had

        if (dishId == rendangId)
        {
            // Simulate a validation error for Rendang dish ID using Kevin's code
            //context.Response.StatusCode = StatusCodes.Status400BadRequest;
            //await context.Response.WriteAsync("Rendang dish cannot be updated.");
            //return; // Short-circuit the pipeline
            return TypedResults.Problem(new()
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Dish is perfect and cannot be changed.",
                Detail = "Rendang dish is already perfect and cannot be updated."
            });
        }

        // invoke the next filter in the pipeline
        var result = await next.Invoke(context);
        return result; // Return the result from the next filter in the pipeline, which Kevin only showed in the next section of his code.
    }
}
