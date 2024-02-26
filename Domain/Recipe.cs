using System.ComponentModel.DataAnnotations;

namespace Domain;

public class Recipe : BaseEntity
{
    [MaxLength(128)]
    public string Name { get; set; } = default!;
    
    [MaxLength(256)]
    public string Description { get; set; } = default!;
    public int Servings { get; set; }
    
    public int TimeInMinutes { get; set; }
    [MaxLength(128)]
    public string Category { get; set; } = default!;
    
    public ICollection<RecipeIngredient>? IngredientsInRecipe { get; set; }
}