using System.IO;
using System.Threading.Tasks;
using Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using RacingSite.Hubs;
using RacingSite.Repositories;

namespace RacingSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRacerBuffer _buffer;
        private readonly IHubContext<RaceHub> _hubContext;

        public HomeController(IRacerBuffer buffer, IHubContext<RaceHub> hubContext, IMessageListener messageListener)
        {
            _buffer = buffer;
            _hubContext = hubContext;
            messageListener.Start();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RealTimePage()
        {
            return View();
        }

        [HttpPut("init")]
        public async Task Initialize()
        {
            var body = (await new StreamReader(Request.Body).ReadToEndAsync()).Split('&');
            var race = JsonConvert.DeserializeObject<Race>(body[0]);
            var racers = JsonConvert.DeserializeObject<Racer[]>(body[1]);

            _buffer.Initialize(race, racers);
            await _hubContext.Clients.All.SendAsync(HubConstants.OnRaceInit, race);
        }
    }
}
