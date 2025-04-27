using AutoMapper;
using MyDishesAPI.Entities;
using MyDishesAPI.Models;

namespace MyDishesAPI.Profiles
{
    public class IngredientProfile : Profile
    {
        public IngredientProfile()
        {
            CreateMap<Ingredient, IngredientDTO>()
                .ForMember(
                    dest => dest.DishId,
                    opt => opt.MapFrom(src => src.Dishes.FirstOrDefault().Id)   // Kevin had "src.Dishes.First().Id"
                );
        }
    }
}
