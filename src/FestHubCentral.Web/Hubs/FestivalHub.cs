using Microsoft.AspNetCore.SignalR;

namespace FestHubCentral.Web.Hubs;

public class FestivalHub : Hub
{
    public async Task SendOrderUpdate(string message)
    {
        await Clients.All.SendAsync("ReceiveOrderUpdate", message);
    }

    public async Task SendInventoryAlert(string message)
    {
        await Clients.All.SendAsync("ReceiveInventoryAlert", message);
    }

    public async Task SendVendorStatusUpdate(int vendorId, bool isOpen)
    {
        await Clients.All.SendAsync("ReceiveVendorStatusUpdate", vendorId, isOpen);
    }

    public async Task SendDashboardUpdate(object data)
    {
        await Clients.All.SendAsync("ReceiveDashboardUpdate", data);
    }

    public async Task NotifyNewOrder(int orderId, decimal amount, string vendorName)
    {
        await Clients.All.SendAsync("ReceiveNewOrder", orderId, amount, vendorName);
    }

    public async Task NotifyLowStock(int productId, string productName, int currentStock)
    {
        await Clients.All.SendAsync("ReceiveLowStockAlert", productId, productName, currentStock);
    }

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
        Console.WriteLine($"Client connected: {Context.ConnectionId}");
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
        Console.WriteLine($"Client disconnected: {Context.ConnectionId}");
    }
}
