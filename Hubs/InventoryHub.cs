using Microsoft.AspNetCore.SignalR;

namespace InventoryManagementSystem.Hubs
{
    public class InventoryHub : Hub
    {
        public async Task NotifyInventoryUpdate(string productName, int newQuantity)
        {
            await Clients.All.SendAsync("ReceiveInventoryUpdate", productName, newQuantity);
        }

        public async Task NotifyNewSale(object sale)
        {
            await Clients.All.SendAsync("ReceiveNewSale", sale);
        }
    }
}