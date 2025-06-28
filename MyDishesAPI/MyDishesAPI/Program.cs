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

builder.Services.AddAuthentication().AddJwtBearer();

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
