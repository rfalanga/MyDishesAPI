using AutoMapper;
using MyDishesAPI.Entities;
using MyDishesAPI.Models;

namespace MyDishesAPI.Profiles;

public class IngredientProfileFromKevin : Profile
{
    public IngredientProfileFromKevin()
    {
        CreateMap<Ingredient, IngredientDTO>()
          .ForMember(
              d => d.DishId,
              o => o.MapFrom(s => s.Dishes.First().Id));
    }
}

