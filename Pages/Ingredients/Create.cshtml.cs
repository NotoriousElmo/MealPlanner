using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL;
using Domain;
using Humanizer;

namespace WebApp.Pages_Ingredients
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
        ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Ingredient Ingredient { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Ingredient.Product = _context.Product.FirstOrDefault(x => x.Id.Equals(Ingredient.ProductId));
            
            _context.Ingredient.Add(Ingredient);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
