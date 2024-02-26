using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages_RecipeIngredients
{
    public class EditModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public EditModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public RecipeIngredient RecipeIngredient { get; set; } = default!;

        [BindProperty] 
        public Ingredient Ingredient { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipeingredient =  await _context.RecipeIngredient
                .Include(x=>x.Ingredient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recipeingredient == null)
            {
                return NotFound();
            }
            var ingredients = _context.Ingredient
                .Include(p => p.Product)
                .Select(x => new 
                { 
                    x.Id, 
                    Description = $"{x.Product!.Name}, {x.Amount} {x.Product.Unit}"
                });
            
            var recipes = _context.Recipe
                .Select(x => new 
                { 
                    x.Id, 
                    Description = $"{x.Name}, {x.Servings}"
                });
            
            RecipeIngredient = recipeingredient;
            Ingredient = RecipeIngredient.Ingredient;

            Console.WriteLine(Ingredient!.Id);
           
            ViewData["IngredientId"] = new SelectList(ingredients, "Id", "Description");
           
            ViewData["RecipeId"] = new SelectList(recipes, "Id", "Description");
            
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Name");

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Ingredient.Id = RecipeIngredient.IngredientId;
            

            var recipeIngredient = _context.RecipeIngredient.FirstOrDefault(x => x.Id.Equals(RecipeIngredient.Id));
            recipeIngredient!.RecipeId = RecipeIngredient.RecipeId;
            recipeIngredient.IngredientId = RecipeIngredient.IngredientId;
            recipeIngredient.Ingredient =
                _context.Ingredient.FirstOrDefault(x => x.Id.Equals(RecipeIngredient.IngredientId));
            recipeIngredient.Recipe = _context.Recipe.FirstOrDefault(x => x.Id.Equals(RecipeIngredient.RecipeId));
            recipeIngredient.Comment = RecipeIngredient.Comment;

            var ingredient = _context.Ingredient.FirstOrDefault(x => x.Id.Equals(Ingredient.Id));

            ingredient!.Amount = Ingredient.Amount;
            ingredient.ProductId = Ingredient.ProductId;
            ingredient.Product = _context.Product.FirstOrDefault(x => x.Id.Equals(Ingredient.ProductId));

            await _context.SaveChangesAsync();

            return RedirectToPage("../ViewRecipe", new {RecipeId = RecipeIngredient.RecipeId});
        }

        private bool RecipeIngredientExists(Guid id)
        {
            return _context.RecipeIngredient.Any(e => e.Id == id);
        }
    }
}
