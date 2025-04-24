using Microsoft.AspNetCore.Mvc.RazorPages;
using InventoryManagementSystem.Data;
using InventoryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Pages.Dashboard
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public int TotalProducts { get; set; }
        public int LowStockCount { get; set; }
        public decimal SalesToday { get; set; }
        public List<Product> LowStockProducts { get; set; } = new();
        public List<Sale> RecentSales { get; set; } = new();

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync()
        {
            TotalProducts = await _context.Products.CountAsync();
            LowStockProducts = await _context.Products.Where(p => p.Quantity <= 5).ToListAsync();
            LowStockCount = LowStockProducts.Count;

            var today = DateTime.Today;
            SalesToday = await _context.Sales
                .Where(s => s.Date >= today)
                .SumAsync(s => s.TotalPrice);

            RecentSales = await _context.Sales
                .Include(s => s.Product)
                .OrderByDescending(s => s.Date)
                .Take(5)
                .ToListAsync();
        }
    }
}
