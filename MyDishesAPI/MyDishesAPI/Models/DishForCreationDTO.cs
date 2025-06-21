using System.ComponentModel.DataAnnotations;

namespace MyDishesAPI.Models;

public class DishForCreationDTO
{
    [Required]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "The name must be between 3 and 100 characters long.")]
    public required string Name { get; set; } = string.Empty;
}
