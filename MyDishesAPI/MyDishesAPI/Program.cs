using Microsoft.EntityFrameworkCore;
using MyDishesAPI.DbContexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// register the DbContext on the container, getting the connection string from appsettings.json
builder.Services.AddDbContext<MyDishesDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("MyDishesDbContext"))); // Kevin had "ConnectionStrings:DishesDBConnectionString"

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapGet("/dishes", (MyDishesDbContext db) =>
{
    return db.Dishes;
});

app.Run();
