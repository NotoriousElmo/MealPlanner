using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages_RecipeIngredients
{
    public class DeleteModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public DeleteModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public RecipeIngredient RecipeIngredient { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipeingredient = await _context.RecipeIngredient.FirstOrDefaultAsync(m => m.Id == id);

            if (recipeingredient == null)
            {
                return NotFound();
            }
            else
            {
                RecipeIngredient = recipeingredient;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipeingredient = await _context.RecipeIngredient.FindAsync(id);
            if (recipeingredient != null)
            {
                RecipeIngredient = recipeingredient;
                _context.RecipeIngredient.Remove(RecipeIngredient);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
