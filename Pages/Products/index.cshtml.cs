using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InventoryManagementSystem.Models;
using InventoryManagementSystem.Data;

namespace InventoryManagementSystem.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public IList<Product> Products { get; set; } = new List<Product>();

        public async Task OnGetAsync()
        {
            // Load products along with their categories
            Products = await _context.Products
                .Include(p => p.Category) // Ensure Category is included
                .Where(p => p.CategoryId != 0) // Ensure products have a valid CategoryId
                .ToListAsync();
        }
    }
}