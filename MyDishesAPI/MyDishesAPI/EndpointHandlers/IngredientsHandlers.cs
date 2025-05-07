namespace MyDishesAPI.EndpointHandlers;

public static class IngredientsHandlers
{
    public static async Task<Ok<IEnumerable<IngredientDTO>>> GetIngredientsAsync(MyDishesDbContext db, IMapper mapper, string? name)
    {
        return TypedResults.Ok(mapper.Map<IEnumerable<IngredientDTO>>(await db.Ingredients
            .Where(i => name == null || i.Name.Contains(name))
            .ToListAsync()));
    }
    public static async Task<Results<NotFound, Ok<IngredientDTO>>> GetIngredientByIdAsync(MyDishesDbContext db, Guid ingredientId, IMapper mapper)
    {
        var ingredient = await db.Ingredients
            .FirstOrDefaultAsync(i => i.Id == ingredientId);
        if (ingredient is null)
        {
            return TypedResults.NotFound();
        }
        return TypedResults.Ok(mapper.Map<IngredientDTO>(ingredient));
    }
}
