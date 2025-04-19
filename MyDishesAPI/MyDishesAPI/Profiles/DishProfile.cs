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
        }
    }
}
