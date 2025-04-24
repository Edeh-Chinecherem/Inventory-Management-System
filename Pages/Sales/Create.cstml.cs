using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using InventoryManagementSystem.Hubs;
using InventoryManagementSystem.Models;
using InventoryManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.Pages.Sales
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<InventoryHub> _hubContext;

        public CreateModel(AppDbContext context, IHubContext<InventoryHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [BindProperty]
        public List<Sale> Sales { get; set; } = new List<Sale>();

        public List<Product> Products { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            
            Products = await _context.Products.ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("Model state is invalid.");
                Products = await _context.Products.ToListAsync();
                return Page();
            }

            foreach (var sale in Sales)
            {
                
                var product = await _context.Products.FindAsync(sale.ProductId);
                if (product == null || product.Quantity < sale.QuantitySold)
                {
                    ModelState.AddModelError("", $"Insufficient stock for product {product?.Name ?? "Unknown"}.");
                    Products = await _context.Products.ToListAsync();
                    return Page();
                }

                // Update product stock
                product.Quantity -= sale.QuantitySold;

                // Compute sale details
                sale.TotalPrice = sale.QuantitySold * product.Price;
                sale.Date = DateTime.Now;

                _context.Sales.Add(sale);

                // Notify via SignalR
                await _hubContext.Clients.All.SendAsync("ReceiveInventoryUpdate", product.Name, product.Quantity);
            }

            await _context.SaveChangesAsync();

            ModelState.Clear(); // Optional: Reset model state

            return RedirectToPage("./Index");
        }
    }
}
