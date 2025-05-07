using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MyDishesAPI.DbContexts;
using MyDishesAPI.Models;

namespace MyDishesAPI.EndpointHandlers;

public static class IngredientsHandlers
{
    public static async Task<Results<NotFound, Ok<IEnumerable<IngredientDTO>>>> GetIngredientByIdAsync(MyDishesDbContext db, Guid ingredientId, IMapper mapper)
    {
        var ingredient = await db.Ingredients
            .FirstOrDefaultAsync(i => i.Id == ingredientId);
        if (ingredient is null)
        {
            return TypedResults.NotFound();
        }
        return TypedResults.Ok(mapper.Map<IEnumerable<IngredientDTO>>(ingredient));
    }
}
