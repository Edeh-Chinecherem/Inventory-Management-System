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
    
        public List<Product> Products { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Load products for the dropdown
            Products = await _context.Products.ToListAsync();
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("Model state is invalid. Returning to page.");
                 // Iterate through ModelState to find invalid fields
    foreach (var entry in ModelState)
    {
        if (entry.Value.Errors.Count > 0) // Check if there are errors for the field
        {
            Console.WriteLine($"Key: {entry.Key}, Errors: {string.Join(", ", entry.Value.Errors.Select(e => e.ErrorMessage))}");
        }
    }
                Products = await _context.Products.ToListAsync();
                return Page();
            }

            foreach (var sale in Sales)
            {
                Console.WriteLine($"Processing sale for product ID: {sale.ProductId}, Quantity Sold: {sale.QuantitySold}");
                var product = await _context.Products.FindAsync(sale.ProductId);
                if (product == null || product.Quantity < sale.QuantitySold)
                {
                    ModelState.AddModelError("", $"Insufficient stock for product {product?.Name ?? "Unknown"}.");
                    Products = await _context.Products.ToListAsync();
                    return Page();
                }

                // Update product quantity
                product.Quantity -= sale.QuantitySold;

                // Calculate total price
                sale.TotalPrice = sale.QuantitySold * product.Price;
                sale.Date = DateTime.Now;

                // Add the sale to the database
                _context.Sales.Add(sale);
             _context.SaveChanges();

                // Notify clients about the inventory update
                await _hubContext.Clients.All.SendAsync("ReceiveInventoryUpdate", product.Name, product.Quantity);
            }


            return RedirectToPage("/Sales/Index");
        }
    }
}