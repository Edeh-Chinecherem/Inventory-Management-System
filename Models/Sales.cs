namespace InventoryManagementSystem.Models;

public class Sale
{
    public int Id { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int QuantitySold { get; set; }
    public decimal TotalPrice { get; set; }
}