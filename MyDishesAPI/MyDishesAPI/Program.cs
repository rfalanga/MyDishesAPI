using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyDishesAPI.DbContexts;
using MyDishesAPI.Entities;
using MyDishesAPI.Extensions;
using MyDishesAPI.Models;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container

// register the DbContext on the container, getting the connection string from appsettings.json
builder.Services.AddDbContext<MyDishesDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("MyDishesDbContext"))); // Kevin had "ConnectionStrings:DishesDBConnectionString"

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddProblemDetails(); // This is for the exception handler

builder.Services.AddAuthentication().AddBearerToken();  // Kevin had AddJwtBearer, but when I tried that, it didn't work. Either I am misssing an assembly reference, or I need to add a package reference to Microsoft.AspNetCore.Authentication.JwtBearer. I don't know which one it is, so I am using AddBearerToken instead.

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
    //app.UseExceptionHandler(configureApplicationBuilder =>
    //{
    //    configureApplicationBuilder.Run(async context =>
    //    {
    //        context.Response.StatusCode = StatusCodes.Status500InternalServerError; // Kevin had context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
    //        context.Response.ContentType = "text/html";  // GH Copilot had "application/json"
    //        await context.Response.WriteAsync("An unexpected error occurred. Please try again later.");
    //    });
    //});
}


app.UseHttpsRedirection();

//var dishesEndpoints = app.MapGroup("/dishes").WithTags("Dishes");   // Kevin didn't have WithTags, but I think I'll keep it for now
//var dishWithGuidIdEndpoints = dishesEndpoints.MapGroup("/dishes/{dishId:guid}"); // And this is what Kevin had
//var ingredientsEndpoints = dishWithGuidIdEndpoints.MapGroup("/dishes/{dishId:guid}/ingredients"); // Kevin had this too

//// Because there is no querystring parameter with the name "name", we don't have to put FromQuery here. Removed [FromQuery].
//dishesEndpoints.MapGet("", async Task<Ok<IEnumerable<DishDTO>>>(MyDishesDbContext db, ClaimsPrincipal claimsPrincipal, IMapper mapper, string? name) =>
//{
//    Console.WriteLine($"User: {claimsPrincipal.Identity?.IsAuthenticated}");

//    return TypedResults.Ok(mapper.Map<IEnumerable<DishDTO>>(await db.Dishes
//        .Where(d => name == null || d.Name.Contains(name))
//        .ToListAsync()));
//});

//dishesEndpoints.MapGet("/{dishName:string}", async (MyDishesDbContext db, IMapper mapper, string dishName) =>
//{
//    return mapper.Map<DishDTO> (await db.Dishes.FirstOrDefaultAsync(d => d.Name == dishName)
//        is Dish dish
//            ? TypedResults.Ok(dish) // Can use TypedResults here, because we have a DTO for the dish name.
//            : Results.NotFound());  // Cannot use TypedResults here, because we don't have a DTO for the dish name.
//});

//ingredientsEndpoints.MapGet("", async Task<Results<NotFound, Ok<IEnumerable<IngredientDTO>>>>(MyDishesDbContext db, IMapper mapper, Guid dishId) =>
//{
//    var dishEntity = await db.Dishes.FirstOrDefaultAsync(d => d.Id == dishId);

//    if (dishEntity is null)
//    {
//        return TypedResults.NotFound();
//    }

//    return TypedResults.Ok(mapper.Map<IEnumerable<IngredientDTO>>((await db.Dishes
//        .Include(d => d.Ingredients)
//        .FirstOrDefaultAsync(d => d.Id == dishId))?.Ingredients));
//});

//dishWithGuidIdEndpoints.MapGet("", async Task<Results<NotFound, Ok<DishDTO>>> (MyDishesDbContext db, IMapper mapper, Guid dishId) =>
//{
//    var dish = await db.Dishes.FirstOrDefaultAsync(d => d.Id == dishId);
//    return dish is not null
//        ? TypedResults.Ok(mapper.Map<DishDTO>(dish))
//        : TypedResults.NotFound();
//}).WithName("GetDish");

//dishesEndpoints.MapPost("", async Task<CreatedAtRoute<DishDTO>> (MyDishesDbContext db, IMapper mapper, DishForCreationDTO dishForCreationDTO) =>
//{
//    var dishEntity = mapper.Map<Dish>(dishForCreationDTO);    // The dishForCreationDTO is from the body of the request
//    db.Add(dishEntity);
//    await db.SaveChangesAsync();

//    var dishToReturn = mapper.Map<DishDTO>(dishEntity);

//    // Kevin wants to simply this code, so he changed it to this
//    return TypedResults.CreatedAtRoute(
//        dishToReturn, "GetDish", new { dishId = dishToReturn.Id }
//        );

//    // The linkGenerator is used to create the URL for the newly created dish
//    //var linkToDish = linkGenerator.GetUriByName(
//    //    httpContext, "GetDish", new { dishId = dishToReturn.Id }
//    //    );

//    //return TypedResults.Created(linkToDish, dishToReturn); // using generated link instead of hardcoded URL
//});

//dishWithGuidIdEndpoints.MapPut("", async Task<Results<NotFound, NoContent>> (MyDishesDbContext db, IMapper mapper, Guid dishId, DishForUpdateDTO dishForUpdateDTO) =>
//{
//    var dishEntity = await db.Dishes.FirstOrDefaultAsync(d => d.Id == dishId);
//    if (dishEntity is null)
//    {
//        return TypedResults.NotFound();
//    }

//    mapper.Map(dishForUpdateDTO, dishEntity);

//    await db.SaveChangesAsync();

//    return TypedResults.NoContent();
//});

//dishWithGuidIdEndpoints.MapDelete("", async Task<Results<NotFound, NoContent>> (MyDishesDbContext db, Guid dishId) =>
//{
//    var dishEntity = await db.Dishes.FirstOrDefaultAsync(d => d.Id == dishId);
//    if (dishEntity is null)
//    {
//        return TypedResults.NotFound();
//    }
//    db.Remove(dishEntity);  // Kevin has db.Dishes.Remove(dishEntity). I don't know why not.
//    //db.Dishes.Remove(dishEntity);   // This is what Kevin had, but it still didn't work
//    await db.SaveChangesAsync();
//    return TypedResults.NoContent();
//});

app.RegisterDishesEndpoints(); 
app.RegisterIngredientsEndpoints(); 

// recreate & migrate the database on each run, for demo purposes
using (var serviceScope = app.Services.GetService<IServiceScopeFactory>()?.CreateScope())
{
    var context = serviceScope?.ServiceProvider.GetRequiredService<MyDishesDbContext>();
    context?.Database.EnsureDeleted();
    context?.Database.Migrate();
}

app.Run();
