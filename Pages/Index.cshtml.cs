using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private DAL.AppDbContext _context;

    public IndexModel(DAL.AppDbContext context, ILogger<IndexModel> logger)
    {
        _context = context;
        _logger = logger;
    }

    [BindProperty] 
    public string? Search { get; set; } = "";
    [BindProperty]
    public bool InRecipeName { get; set; }

    [BindProperty]
    public bool CookTime { get; set; }
    
    [BindProperty]
    public bool Contains { get; set; }
    
    [BindProperty]
    public bool DoesntContain { get; set; }
    [BindProperty]
    public bool InCategory { get; set; }
    [BindProperty]
    public bool InServingSize { get; set; }
    
    public IList<Domain.Recipe> Recipes { get;set; } = default!;
    

    public async Task<IActionResult> OnGet()
    {
        Recipes = await _context.Recipe            
            .Where(x => x.IngredientsInRecipe
            .All(i=> i.Ingredient!.Product!.Amount >= i.Ingredient.Amount)).ToListAsync();

        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        var query = _context.Recipe
            .Where(x => x.IngredientsInRecipe
                .All(i=> i.Ingredient!.Product!.Amount >= i.Ingredient.Amount))
            .Include(x => x.IngredientsInRecipe)!
            .ThenInclude(x => x.Ingredient)
            .ThenInclude(x => x!.Product)
            .AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(Search))
        {
            var values = Search.Split("/");

            if (InRecipeName)
            {
                var name = values[0];
                Console.WriteLine("Name: " + name);
                query = query.Where(x => x.Name.ToLower().Contains(name!.ToLower())).AsQueryable();

                values = values.Skip(1).ToArray();
            }


            if (CookTime)
            {
                var time = int.Parse(values[0]);
                
                query = query.Where(x => x.TimeInMinutes <= time);

                foreach (var VARIABLE in query)
                {
                    Console.WriteLine("Time: " + time + " queryTime: " + VARIABLE.TimeInMinutes + " : " +
                                      (VARIABLE.TimeInMinutes <= time));
                }
                
                values = values.Skip(1).ToArray();
            }

            if (Contains)
            {
                var contains = values[0].Split(',');
                
                Console.WriteLine("Contains: " + contains);

                query = query
                    .Where(x => contains.All(c => x.IngredientsInRecipe!
                        .Any(i => i.Ingredient!.Product!.Name.ToLower().Contains(c.ToLower()))));

                values = values.Skip(1).ToArray();

            }

            if (DoesntContain)
            {
                var notContains = values[0].Split(",");
                
                Console.WriteLine("NotContains: " + notContains);

                query = query                        
                    .Where(x => !notContains.All(c => x.IngredientsInRecipe!
                    .Any(i => i.Ingredient!.Product!.Name.ToLower().Contains(c.ToLower()))));
                
                values = values.Skip(1).ToArray();
            }

            if (InCategory)
            {
                var category = values[0].Split(",");
                
                Console.WriteLine("Category: " + category);

                query = query.Where(x => category.Any(c => x.Category.ToLower().Contains(c.ToLower())));

                values = values.Skip(1).ToArray();
            }

            if (InServingSize)
            {
                var size = int.Parse(values[0]);

                query = query.Where(x => x.Servings >= size);

                values = values.Skip(1).ToArray();
            }

        }

        Recipes = query.ToList();
        
        return Page();
    }
    
    public IActionResult OnPostReset()
    {
        Search = "";
        InRecipeName = false;
        CookTime = false;
        Contains = false;
        DoesntContain = false;
        InCategory = false;

        return RedirectToPage();
    }

}