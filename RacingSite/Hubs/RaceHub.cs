using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace RacingSite.Hubs
{
    public class RaceHub : Hub
    {
        public Task NotifyAboutCheckpointPassing(string v)
        {
            return Clients.All.SendAsync(HubConstants.OnCheckpointPassed, new { p1 = 17, p2 = v });
        }
    }
}