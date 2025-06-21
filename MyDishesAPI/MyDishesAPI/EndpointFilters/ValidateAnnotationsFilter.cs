using MiniValidation;
using MyDishesAPI.Models;

namespace MyDishesAPI.EndpointFilters;

public class ValidateAnnotationsFilter : IEndpointFilter
{
    public ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var dishForCreationDTO = context.GetArgument<DishForCreationDTO>(2);

        if (!MiniValidator.TryValidate(dishForCreationDTO, out var validationErrors))
        {
            // If validation fails, return a BadRequest with the validation validationErrors
            return ValueTask.FromResult<object?>(TypedResults.ValidationProblem(validationErrors));
        }
    }
}
