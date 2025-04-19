using AutoMapper;
using MyDishesAPI.Entities;
using MyDishesAPI.Models;

namespace MyDishesAPI.Profiles
{
    public class DishProfile : Profile
    {
        public DishProfile()
        {
            CreateMap<Dish, DishDTO>()
                .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.Ingredients.Select(i => i.Name)))
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories.Select(c => c.Name)))
                .ForMember(dest => dest.DishType, opt => opt.MapFrom(src => src.DishType.Name))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
        }
    }
}
