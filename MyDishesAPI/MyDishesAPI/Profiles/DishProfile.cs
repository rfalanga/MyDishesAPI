using AutoMapper;
using MyDishesAPI.Entities;
using MyDishesAPI.Models;

namespace MyDishesAPI.Profiles
{
    public class DishProfile : Profile
    {
        public DishProfile()
        {
            CreateMap<Dish, DishDTO>();
            CreateMap<DishForCreationDTO, Dish>()
                .ForMember(d => d.Id, opt => opt.MapFrom(_ => Guid.NewGuid())); // Generate a new Guid for the Id property, this was suggested by GH Copilot; Kevin did NOT have this!
        }
    }
}
