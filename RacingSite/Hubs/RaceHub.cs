using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace RacingSite.Hubs
{
    public class RaceHub : Hub
    {
        public Task NotifyAboutCheckpointPassing()
        {
            return Clients.All.SendAsync("OnCheckpointPassed", new { p1 = 17, p2 = "XYZ" });
        }
    }
}