namespace MyDishesAPI.EndpointFilters;

public class RendangDishIsLockedFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var dishId = context.GetArgument<Guid>(2);  // This is just one way of getting the argument from the context.
        var rendangId = new Guid("f7a5c8d7-6b2e-4d3e-b6f2-4c1b8f9f6a2b"); // Example dish ID for Rendang, which is different from the one in Kevin's code.

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
