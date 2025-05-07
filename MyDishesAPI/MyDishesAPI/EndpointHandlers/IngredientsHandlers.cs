using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MyDishesAPI.DbContexts;
using MyDishesAPI.Models;

namespace MyDishesAPI.EndpointHandlers;

public static class IngredientsHandlers
{
    public static async Task<Results<NotFound, Ok<IEnumerable<IngredientDTO>>>> GetIngredientByIdAsync(MyDishesDbContext db, Guid dishId, IMapper mapper)
    {
        // This is what GitHub Copilot suggested. Note: It is NOT what Kevin had in his code.
        var ingredient = await db.Ingredients
            .FirstOrDefaultAsync(i => i.Id == dishId);
        if (ingredient is null)
        {
            return TypedResults.NotFound();
        }
        return TypedResults.Ok(mapper.Map<IEnumerable<IngredientDTO>>(ingredient));

        // This is what Kevin had in his code. Note: It is NOT what GitHub Copilot suggested.
        //var dishEntity = await db.Dishes.FirstOrDefaultAsync(d => d.Id == dishId);
        //if (dishEntity == null)
        //{
        //    return TypedResults.NotFound();
        //}

        //return TypedResults.Ok(mapper.Map<IEnumerable<IngredientDTO>>((await db.Dishes
        //    .Include(d => d.Ingredients)
        //    .FirstOrDefaultAsync(d => d.Id == dishId))?.Ingredients));
    }
}
