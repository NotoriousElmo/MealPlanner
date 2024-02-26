using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages_RecipeIngredients
{
    public class CreateModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public CreateModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {

            var ingredients = _context.Ingredient
                .Include(x => x.Product)
                .Select(x => new
                {
                    x.Id,
                    Description = $"{x.Product!.Name}, {x.Amount} {x.Product.Unit}"
                });
            
            ViewData["IngredientId"] = new SelectList(ingredients, "Id", "Description");
            ViewData["RecipeId"] = new SelectList(_context.Recipe, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public RecipeIngredient RecipeIngredient { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            RecipeIngredient.Recipe = 
                _context.Recipe.FirstOrDefault(x => x.Id.Equals(RecipeIngredient.RecipeId));
            RecipeIngredient.Ingredient =
                _context.Ingredient.FirstOrDefault(x => x.Id.Equals(RecipeIngredient.RecipeId));

            _context.RecipeIngredient.Add(RecipeIngredient);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
