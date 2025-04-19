namespace MyDishesAPI.Models
{
    // This is an ingredient that belows to a single dish. This will also get rid of the cyclic references.
    public class IngredientDTO
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public Guid DishId { get; set; }
    }
}
