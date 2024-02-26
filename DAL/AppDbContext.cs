using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class AppDbContext : DbContext
{
    public DbSet<Ingredient> Ingredient { get; set; } = default!;
    public DbSet<Product> Product { get; set; } = default!;
    public DbSet<Recipe> Recipe { get; set; } = default!;
    public DbSet<RecipeIngredient> RecipeIngredient { get; set; } = default!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

}