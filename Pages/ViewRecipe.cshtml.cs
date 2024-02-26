using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages;

public class ViewRecipe : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private DAL.AppDbContext _context;

    public ViewRecipe(DAL.AppDbContext context, ILogger<IndexModel> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    [BindProperty(SupportsGet = true)]
    public Guid RecipeId { get; set; }

    public Recipe Recipe { get; set; } = default!;
    public IList<RecipeIngredient> RecipeIngredients { get; set; } = default!;

    public void OnGet()
    {
        Recipe = _context.Recipe.FirstOrDefault(x => x.Id.Equals(RecipeId));
        RecipeIngredients = _context.RecipeIngredient
            .Include(r => r.Ingredient)
            .ThenInclude(i => i!.Product)
            .Include(r => r.Recipe)
            .Where(x => x.RecipeId.Equals(RecipeId)).ToList();
    }
}