using System.ComponentModel.DataAnnotations;

namespace Domain;

public class Product : BaseEntity
{
    [MaxLength(128)]
    public string Name { get; set; } = default!;
    
    public EUnit Unit { get; set; }
    public int Amount { get; set; }

    public string Location { get; set; } = default!;

    [MaxLength(128)]
    public string Category { get; set; } = default!;

    public ICollection<Ingredient>? Ingredients { get; set; }
}