using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyDishesAPI.DbContexts;
using MyDishesAPI.Entities;
using MyDishesAPI.Models;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// register the DbContext on the container, getting the connection string from appsettings.json
builder.Services.AddDbContext<MyDishesDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("MyDishesDbContext"))); // Kevin had "ConnectionStrings:DishesDBConnectionString"

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

// Because there is no querystring parameter with the name "name", we don't have to put FromQuery here. Removed [FromQuery].
app.MapGet("/dishes", async Task<Ok<IEnumerable<DishDTO>>>(MyDishesDbContext db, ClaimsPrincipal claimsPrincipal, IMapper mapper, string? name) =>
{
    Console.WriteLine($"User: {claimsPrincipal.Identity?.IsAuthenticated}");

    return TypedResults.Ok(mapper.Map<IEnumerable<DishDTO>>(await db.Dishes
        .Where(d => name == null || d.Name.Contains(name))
        .ToListAsync()));
});

app.MapGet("/dishes/{dishName:string}", async (MyDishesDbContext db, IMapper mapper, string dishName) =>
{
    return mapper.Map<DishDTO> (await db.Dishes.FirstOrDefaultAsync(d => d.Name == dishName)
        is Dish dish
            ? TypedResults.Ok(dish) // Can use TypedResults here, because we have a DTO for the dish name.
            : Results.NotFound());  // Cannot use TypedResults here, because we don't have a DTO for the dish name.
});

app.MapGet("/dishes/{dishId}/ingredients", async Task<Results<NotFound, Ok<IEnumerable<IngredientDTO>>>>(MyDishesDbContext db, IMapper mapper, Guid dishId) =>
{
    var dishEntity = await db.Dishes.FirstOrDefaultAsync(d => d.Id == dishId);

    if (dishEntity is null)
    {
        return TypedResults.NotFound();
    }

    return TypedResults.Ok(mapper.Map<IEnumerable<IngredientDTO>>((await db.Dishes
        .Include(d => d.Ingredients)
        .FirstOrDefaultAsync(d => d.Id == dishId))?.Ingredients));
});

app.MapGet("/dishes/{dishId:guid}", async Task<Results<NotFound, Ok<DishDTO>>> (MyDishesDbContext db, IMapper mapper, Guid dishId) =>
{
    var dish = await db.Dishes.FirstOrDefaultAsync(d => d.Id == dishId);
    return dish is not null
        ? TypedResults.Ok(mapper.Map<DishDTO>(dish))
        : TypedResults.NotFound();
});

// recreate & migrate the database on each run, for demo purposes
using (var serviceScope = app.Services.GetService<IServiceScopeFactory>()?.CreateScope())
{
    var context = serviceScope?.ServiceProvider.GetRequiredService<MyDishesDbContext>();
    context?.Database.EnsureDeleted();
    context?.Database.Migrate();
}

app.Run();
