using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.SignalR;

namespace DayPay.SignalRHubs;

[HubRoute("/daypay/signalr-hubs/notification")]
public class NotificationHub : AbpHub
{
    public override async Task OnConnectedAsync()
    {
        await Clients.All.SendAsync($"Welcome {Context.ConnectionId} to DayPay Notification!");

        await base.OnConnectedAsync();
    }
}
