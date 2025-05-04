using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MyDishesAPI.DbContexts;
using MyDishesAPI.Entities;
using MyDishesAPI.Models;
using System.Security.Claims;

namespace MyDishesAPI.EndpointHandlers;

public static class DishesHandlers
{
    public static async Task<Ok<IEnumerable<DishDTO>>> GetDishesAsync(MyDishesDbContext db, ClaimsPrincipal claimsPrincipal, IMapper mapper, string? name)
    {
        Console.WriteLine($"User: {claimsPrincipal.Identity?.IsAuthenticated}");

        return TypedResults.Ok(mapper.Map<IEnumerable<DishDTO>>(await db.Dishes
            .Where(d => name == null || d.Name.Contains(name))
            .ToListAsync()));
    }

    public static async Task<Results<NotFound, Ok<DishDTO>>> GetDishByIdAsync(MyDishesDbContext db, Guid dishId, IMapper mapper)
    {
        var dish = await db.Dishes
            .Include(d => d.Ingredients)
            .FirstOrDefaultAsync(d => d.Id == dishId);
        if (dish is null)
        {
            return TypedResults.NotFound();
        }
        return TypedResults.Ok(mapper.Map<DishDTO>(dish));
    }

    public static async Task<Results<NotFound<DishDTO>, Ok<DishDTO>>> GetDishByNameAsync(MyDishesDbContext db, string dishName, IMapper mapper)
    {
        var dish = await db.Dishes
            .FirstOrDefaultAsync(d => d.Name == dishName);
        if (dish is null)
        {
            return TypedResults.NotFound<DishDTO>(null); // Provide a null value to satisfy the required parameter
        }
        return TypedResults.Ok(mapper.Map<DishDTO>(dish));
    }

    public static async Task<CreatedAtRoute<DishDTO>> CreateDishAsync(MyDishesDbContext db, DishForCreationDTO dishForCreationDto, IMapper mapper)
    {
        var dishEntity = mapper.Map<Dish>(dishForCreationDto);
        await db.Dishes.AddAsync(dishEntity);
        await db.SaveChangesAsync();

        var dishToReturn = mapper.Map<DishDTO>(dishEntity);

        return TypedResults.CreatedAtRoute(
            routeName: "GetDish", // Specify the route name if applicable
            routeValues: new { id = dishToReturn.Id }, // Correctly pass route values
            value: dishToReturn // Pass the DishDTO as the value
        );
    }

    public static async Task<Results<NotFound, NoContent>> UpdateDishAsync(MyDishesDbContext db, Guid dishId, DishForUpdateDTO dishForUpdateDto)
    {
        var dishEntity = await db.Dishes
            .FirstOrDefaultAsync(d => d.Id == dishId);
        if (dishEntity is null)
        {
            return TypedResults.NotFound();
        }

        dishEntity.Name = dishForUpdateDto.Name;
        // Kevin had mapper.Map(dishForUpdateDto, dishEntity) here, however GH's Copilot didn't include mapper, so I'm leaving it out
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }
}

