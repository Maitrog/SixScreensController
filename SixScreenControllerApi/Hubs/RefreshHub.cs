using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SixScreenControllerApi.Hubs
{
    public class RefreshHub : Hub
    {
        public Task SendRefresh(int screenNumber)
        {
            return Clients.All.SendAsync("Refresh", screenNumber);
        }

        [HubMethodName("ChangeBackground")]
        public Task SendChangeBackground(int screenNumber)
        {
            return Clients.All.SendAsync("ChangeBackground", screenNumber);
        }

        [HubMethodName("Ping")]
        public Task SendOnlineScreen(string screenNumber)
        {
            return Clients.Others.SendAsync("Ping", screenNumber);
        }
    }
}
