using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MyDishesAPI.DbContexts;
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
}

