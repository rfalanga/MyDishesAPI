﻿using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MyDishesAPI.DbContexts;
using MyDishesAPI.Entities;
using MyDishesAPI.Models;
using System.Security.Claims;

namespace MyDishesAPI.EndpointHandlers;

public static class DishesHandlers
{
    public static async Task<Ok<IEnumerable<DishDTO>>> GetDishesAsync(MyDishesDbContext db, ClaimsPrincipal claimsPrincipal, IMapper mapper, ILogger<DishDTO> logger, string? name)
    {
        Console.WriteLine($"User: {claimsPrincipal.Identity?.IsAuthenticated}");

        logger.LogInformation("Getting the dishes ...");

        return TypedResults.Ok(mapper.Map<IEnumerable<DishDTO>>(await db.Dishes
            .Where(d => name == null || d.Name.Contains(name))
            .ToListAsync()));   // TODO: Getting An exception of type 'Microsoft.Data.Sqlite.SqliteException' occurred in System.Private.CoreLib.dll but was not handled in user code SQLite Error 1: 'no such table: Dishes'.
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
        await db.SaveChangesAsync();    // This raises an inner exception of SqliteException: SQLite Error 1: 'no such table: Dishes'.

        var dishToReturn = mapper.Map<DishDTO>(dishEntity);

        return TypedResults.CreatedAtRoute(
            routeName: "GetDish", // Specify the route name if applicable
            routeValues: new { id = dishToReturn.Id }, // Correctly pass route values
            value: dishToReturn // Pass the DishDTO as the value
        );
    }

    public static async Task<Results<NotFound, NoContent>> UpdateDishAsync(MyDishesDbContext db, IMapper mapper, Guid dishId, DishForUpdateDTO dishForUpdateDTO)
    {
        var dishEntity = await db.Dishes
            .FirstOrDefaultAsync(d => d.Id == dishId);
        if (dishEntity is null)
        {
            return TypedResults.NotFound();
        }

        //dishEntity.Name = dishForUpdateDto.Name;    // I'm putting back in the line that Kevin had for mapper
        mapper.Map(dishForUpdateDTO, dishEntity);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }

    public static async Task<Results<NotFound, NoContent>> DeleteDishAsync(MyDishesDbContext db, Guid dishId)
    {
        var dishEntity = await db.Dishes
            .FirstOrDefaultAsync(d => d.Id == dishId);
        if (dishEntity is null)
        {
            return TypedResults.NotFound();
        }

        db.Dishes.Remove(dishEntity);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }
}

