using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages;

public class AddIngredient : PageModel
{
    private readonly DAL.AppDbContext _context;

    public AddIngredient(DAL.AppDbContext context)
    {
        _context = context;
    }

    [BindProperty(SupportsGet = true)] 
    public Guid RecipeId { get; set; }

    public IActionResult OnGet()
    {

        var products = _context.Product
            .Select(x => new
            {
                x.Id,
                Description = $"{x.Name}, {x.Amount} {x.Unit}"
            });
            
        ViewData["ProductId"] = new SelectList(products, "Id", "Description");
        ViewData["RecipeId"] = new SelectList(_context.Recipe, "Id", "Name");
        return Page();
    }

    [BindProperty] 
    public Product Product { get; set; } = default!;

    [BindProperty]
    public RecipeIngredient RecipeIngredient { get; set; } = default!;

    [BindProperty] 
    public int Amount { get; set; }

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
        // if (!ModelState.IsValid)
        // {
        //     return Page();
        // }

        var recipe = _context.Recipe.FirstOrDefault(x => x.Id.Equals(RecipeIngredient.RecipeId));

        RecipeIngredient.Recipe = recipe;

        var ingredient = new Ingredient
        {
            ProductId = Product.Id,
            Product = _context.Product.FirstOrDefault(x=>x.Id.Equals(Product.Id)),
            Amount = Amount,
            IngredientInRecipes = new List<RecipeIngredient>()
        };
        ingredient.IngredientInRecipes.Add(RecipeIngredient);
        _context.Ingredient.Add(ingredient);

        RecipeIngredient.RecipeId = RecipeId;
        RecipeIngredient.IngredientId = ingredient.Id;
        RecipeIngredient.Ingredient = ingredient;

        _context.RecipeIngredient.Add(RecipeIngredient);

        await _context.SaveChangesAsync();

        return RedirectToPage("/ViewRecipe", new {RecipeId = RecipeId});
    }
}
