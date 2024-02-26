using System.ComponentModel.DataAnnotations;

namespace Domain;

public class RecipeIngredient : BaseEntity
{
    public Guid RecipeId { get; set; }
    public Guid IngredientId { get; set; }
    
    public Recipe? Recipe { get; set; }
    public Ingredient? Ingredient { get; set; }
    
    [MaxLength(256)]
    public string Comment { get; set; } = default!;
}