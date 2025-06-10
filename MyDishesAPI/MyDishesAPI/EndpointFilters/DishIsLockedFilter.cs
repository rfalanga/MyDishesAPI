namespace MyDishesAPI.EndpointFilters;

public class DishIsLockedFilter : IEndpointFilter
{
    private readonly Guid _lockedDishId;

    public DishIsLockedFilter(Guid lockedDishId)
    {
        _lockedDishId = lockedDishId;
    }

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

        if (dishId == _lockedDishId)
        {
            return TypedResults.Problem(new()
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Dish is perfect and cannot be changed.",
                Detail = "You cannot update or delete perfection."
            });
        }

        // invoke the next filter in the pipeline
        var result = await next.Invoke(context);
        return result; // Return the result from the next filter in the pipeline, which Kevin only showed in the next section of his code.
    }
}
