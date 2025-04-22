using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InventoryManagementSystem.Models;
using InventoryManagementSystem.Data;

namespace InventoryManagementSystem.Pages.Sales
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public IList<Sale> Sales { get; set; } = new List<Sale>();

        public async Task OnGetAsync()
        {
            // Load sales with related product details
            Sales = await _context.Sales
                .Include(s => s.Product)
                .ToListAsync();
                Console.WriteLine("Sales loaded successfully.");
        }
    }
}