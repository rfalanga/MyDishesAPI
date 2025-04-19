using AutoMapper;
using MyDishesAPI.Entities;
using MyDishesAPI.Models;

namespace MyDishesAPI
{
    public class IngredientProfile : Profile
    {
        public IngredientProfile()
        {
            CreateMap<Ingredient, IngredientDTO>()
                .ForMember(dest => dest.IngredientId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.IngredientName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.IngredientDescription, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.IngredientType, opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.IngredientImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
                .ForMember(dest => dest.IngredientQuantity, opt => opt.MapFrom(src => src.Quantity));
        }
    }
}
