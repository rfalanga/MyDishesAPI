using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyDishesAPI.DbContexts;
using MyDishesAPI.Entities;
using MyDishesAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// register the DbContext on the container, getting the connection string from appsettings.json
builder.Services.AddDbContext<MyDishesDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("MyDishesDbContext"))); // Kevin had "ConnectionStrings:DishesDBConnectionString"

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapGet("/dishes", async (MyDishesDbContext db) =>
{
    return await db.Dishes.ToListAsync();
});

app.MapGet("/dishes/{dishId:guid}", async (MyDishesDbContext db, IMapper mapper, Guid dishId) =>
{
    var dish = await db.Dishes.FirstOrDefaultAsync(d => d.Id == dishId);
    return dish is not null
        ? Results.Ok(mapper.Map<DishDTO>(dish))
        : Results.NotFound();
});

app.MapGet("/dishes/{dishId}/ingredients", async (MyDishesDbContext db, Guid dishId) =>
{
    return (await db.Dishes
        .Include(d => d.Ingredients)
        .FirstOrDefaultAsync(d => d.Id == dishId))?.Ingredients;    // Note: this can result in an infinite loop, which will resolved later.
});

app.MapGet("/dishes/{dishName:string}", async (MyDishesDbContext db, string dishName) =>
{
    return await db.Dishes.FirstOrDefaultAsync(d => d.Name == dishName)
        is Dish dish
            ? Results.Ok(dish)
            : Results.NotFound();
});

// recreate & migrate the database on each run, for demo purposes
using (var serviceScope = app.Services.GetService<IServiceScopeFactory>()?.CreateScope())
{
    var context = serviceScope?.ServiceProvider.GetRequiredService<MyDishesDbContext>();
    context?.Database.EnsureDeleted();
    context?.Database.Migrate();
}

app.Run();
