﻿using MyDishesAPI.EndpointHandlers;
using MyDishesAPI.Models;

namespace MyDishesAPI.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static void RegisterDishesEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        // This code was generated by GitHub Copilot. It is NOT what Kevin had in his code.
        endpointRouteBuilder.MapGet("/dishes", DishesHandlers.GetDishesAsync)
            .WithName("GetDishes")
            .Produces<IEnumerable<DishDTO>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
        endpointRouteBuilder.MapGet("/dishes/{dishId:guid}", DishesHandlers.GetDishByIdAsync)
            .WithName("GetDish")
            .Produces<DishDTO>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
        endpointRouteBuilder.MapGet("/dishes/name/{dishName}", DishesHandlers.GetDishByNameAsync)
            .WithName("GetDishByName")
            .Produces<DishDTO>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
        endpointRouteBuilder.MapPost("/dishes", DishesHandlers.CreateDishAsync)
            .WithName("CreateDish")
            .Accepts<DishForCreationDTO>("application/json")
            .Produces<DishDTO>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);
        endpointRouteBuilder.MapPut("/dishes/{dishId:guid}", DishesHandlers.UpdateDishAsync)
            .WithName("UpdateDish")
            .Accepts<DishForUpdateDTO>("application/json")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);
        endpointRouteBuilder.MapDelete("", DishesHandlers.DeleteDishAsync);    // from Kevin's code
    }

    public static void RegisterIngredientsEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        // This code was generated by GitHub Copilot. It is NOT what Kevin had in his code.
        endpointRouteBuilder.MapGet("/ingredients", IngredientsHandlers.GetIngredientByIdAsync)
            .WithName("GetAllIngredients")  // Changed from "GetIngredients" to avoid duplication
            .Produces<IEnumerable<IngredientDTO>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
        endpointRouteBuilder.MapGet("/ingredients/{ingredientId:guid}", IngredientsHandlers.GetIngredientByIdAsync)
            .WithName("GetIngredientById")
            .Produces<IngredientDTO>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
        //endpointRouteBuilder.MapPost("/ingredients", IngredientsHandlers.CreateIngredientAsync)
        //    .WithName("CreateIngredient")
        //    .Accepts<IngredientForCreationDTO>("application/json")
        //    .Produces<IngredientDTO>(StatusCodes.Status201Created)
        //    .Produces(StatusCodes.Status400BadRequest);
        //endpointRouteBuilder.MapPut("/ingredients/{ingredientId:guid}", IngredientsHandlers.UpdateIngredientAsync)
        //    .WithName("UpdateIngredient")
        //    .Accepts<IngredientForUpdateDTO>("application/json")
        //    .Produces(StatusCodes.Status204NoContent)
        //    .Produces(StatusCodes.Status404NotFound);

        // This is what Kevin had in his code.
        //var ingredientsEndpoints = endpointRouteBuilder.MapGroup("/dishes/{dishId:guid}/ingredients");

        //ingredientsEndpoints.MapGet("", IngredientsHandlers.GetIngredientByIdAsync)
        //    .WithName("GetIngredients")
        //    .Produces<IEnumerable<IngredientDTO>>(StatusCodes.Status200OK)
        //    .Produces(StatusCodes.Status401Unauthorized);

        // Kevin is adding this code to illustrate handling exceptions with the Developer Exception Page Middleware.
        var ingredientsEndpoints = endpointRouteBuilder.MapGroup("/dishes/{dishId:guid}/ingredients");

        ingredientsEndpoints.MapGet("", IngredientsHandlers.GetIngredientByIdAsync)
            .WithName("GetDishIngredients")  // Changed from "GetIngredients" to avoid duplication
            .Produces<IEnumerable<IngredientDTO>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
        ingredientsEndpoints.MapPost("", () =>
        {
            throw new Exception("This is a test exception for the Developer Exception Page Middleware.");
        });
    }
}
