using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InventoryManagementSystem.Models;
using InventoryManagementSystem.Data;

namespace InventoryManagementSystem.Pages.Products
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;

        public CreateModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Product Product { get; set; }

        [BindProperty]
        public string SelectedCategory { get; set; } // Bind the selected category

        public SelectList CategoryOptions { get; set; } // Dropdown options

        public async Task<IActionResult> OnGetAsync()
        {
            // Predefined category options
            var predefinedCategories = new List<string>
            {
                "Fresh Produce",
                "Dairy Products",
                "Meat and Seafood",
                "Bakery Items",
                "Beverages",
                "Frozen Foods",
                "Pantry Staples",
                "Snacks",
                "Canned and Jarred Goods",
                "Health and Beauty",
                "Household Supplies",
                "Baby Products",
                "Pet Supplies",
                "International Foods",
                "Miscellaneous"
            };

            // Populate the dropdown
            CategoryOptions = new SelectList(predefinedCategories);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Repopulate the dropdown if validation fails
                var predefinedCategories = new List<string>
                {
                    "Fresh Produce",
                    "Dairy Products",
                    "Meat and Seafood",
                    "Bakery Items",
                    "Beverages",
                    "Frozen Foods",
                    "Pantry Staples",
                    "Snacks",
                    "Canned and Jarred Goods",
                    "Health and Beauty",
                    "Household Supplies",
                    "Baby Products",
                    "Pet Supplies",
                    "International Foods",
                    "Miscellaneous"
                };
                CategoryOptions = new SelectList(predefinedCategories);
                return Page();
            }

            // Check if the category exists in the database
            var category = await _context.Categories
                .AsNoTracking() // Avoid tracking since we only need to check existence
                .FirstOrDefaultAsync(c => c.Name == SelectedCategory);

            if (category == null)
            {
                // Create a new category if it doesn't exist
                category = new Category { Name = SelectedCategory };
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
            }

            // Assign the category ID to the product
            Product.CategoryId = category.Id;

            // Add the product to the database
            _context.Products.Add(Product);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}