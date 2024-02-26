using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages;

public class ChangeServings : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private DAL.AppDbContext _context;

    public ChangeServings(DAL.AppDbContext context, ILogger<IndexModel> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    [BindProperty(SupportsGet = true)]
    public Guid RecipeId { get; set; }
    [BindProperty]
    public int OldServings { get; set; }

    [BindProperty] 
    public Recipe Recipe { get; set; } = default!;
    
    
    public void OnGet()
    {
        Recipe = _context.Recipe
            .FirstOrDefault(x => x.Id.Equals(RecipeId)) ?? throw new InvalidOperationException();
        OldServings = Recipe.Servings;
    }
    
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var ingredients = _context.RecipeIngredient
            .Where(x => x.RecipeId.Equals(RecipeId))
            .Include(x=>x.Ingredient);

        var difference = (float)OldServings / Recipe.Servings;

        Console.WriteLine(OldServings);
        Console.WriteLine(Recipe.Servings);

        Console.WriteLine("difference: " + difference);
    
        foreach (var recipeIngredient in ingredients)
        {
            recipeIngredient.Ingredient!.Amount = (int)(recipeIngredient.Ingredient!.Amount / difference);
            if (recipeIngredient.Ingredient!.Amount < 1)
            {
                recipeIngredient.Ingredient!.Amount = 1;
            }
        }

        var recipe = _context.Recipe.FirstOrDefault(x => x.Id.Equals(RecipeId));
        
        Console.WriteLine("recipe: " + recipe);


        if (recipe != null)
        {
            recipe.Servings = Recipe.Servings;

            await _context.SaveChangesAsync();
        }

        return RedirectToPage("/ViewRecipe", new { RecipeId = RecipeId });
    }
    
    

}