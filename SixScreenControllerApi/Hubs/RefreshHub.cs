using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SixScreenControllerApi.Hubs
{
    public class RefreshHub: Hub
    {
        public Task SendRefresh(int screenNumber)
        {
            return Clients.All.SendAsync("Refresh", screenNumber);
        }
    }
}
