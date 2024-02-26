namespace Domain;

public class Ingredient : BaseEntity
{
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }

    public int Amount { get; set; }
    
    public ICollection<RecipeIngredient>? IngredientInRecipes { get; set; }
}