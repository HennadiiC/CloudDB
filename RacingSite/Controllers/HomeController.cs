using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Core;
using Microsoft.AspNetCore.Http;
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

        public HomeController(IRacerBuffer buffer, IHubContext<RaceHub> hubContext)
        {
            _buffer = buffer;
            _hubContext = hubContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RealTimePage()
        {
            return View();
        }

        public IActionResult TestOne()
        {
            var race = new Race
            {
                Id = 1,
                Name = "Race 1",
                Checkpoints = new[]
                {
                    new Checkpoint
                    {
                        Id = 2,
                        Name = "Checkpoint 2"
                    },
                    new Checkpoint
                    {
                        Id = 3,
                        Name = "Checkp 3"
                    },
                }
            };
            var racers = new[]
            {
                new Racer { Id = 4, Name = "Racer 4" },
                new Racer { Id = 5, Name = "Racer 5" },
            };
            _hubContext.Clients.All.SendAsync(HubConstants.OnRaceInit, race);

            _buffer.Initialize(race, racers);

            _buffer.AddCheckpointPassing(new CheckpointPassing
            {
                Id = 6,
                RacerId = racers[0].Id,
                CheckpointId = race.Checkpoints[0].Id,
                Time = DateTime.Now,
            });
            _hubContext.Clients.All.SendAsync(HubConstants.OnCheckpointPassed, _buffer.RacersCurrentStates);

            _buffer.AddCheckpointPassing(new CheckpointPassing
            {
                Id = 7,
                RacerId = racers[1].Id,
                CheckpointId = race.Checkpoints[0].Id,
                Time = DateTime.Now,
            });
            _hubContext.Clients.All.SendAsync(HubConstants.OnCheckpointPassed, _buffer.RacersCurrentStates);



            Thread.Sleep(5 * 1000);



            _buffer.AddCheckpointPassing(new CheckpointPassing
            {
                Id = 7,
                RacerId = racers[1].Id,
                CheckpointId = race.Checkpoints[1].Id,
                Time = DateTime.Now,
            });
            _hubContext.Clients.All.SendAsync(HubConstants.OnCheckpointPassed, _buffer.RacersCurrentStates);




            Thread.Sleep(1 * 1000);


            _buffer.AddCheckpointPassing(new CheckpointPassing
            {
                Id = 7,
                RacerId = racers[0].Id,
                CheckpointId = race.Checkpoints[1].Id,
                Time = DateTime.Now,
            });
            _hubContext.Clients.All.SendAsync(HubConstants.OnCheckpointPassed, _buffer.RacersCurrentStates);


            return RedirectToAction("Index");
        }

        [HttpPut("init")]
        public async Task Initialize()
        {
            var body = (await new StreamReader(Request.Body).ReadToEndAsync()).Split('&');
            var race = JsonConvert.DeserializeObject<Race>(body[0]);
            var racers = JsonConvert.DeserializeObject<Racer[]>(body[1]);

            await _hubContext.Clients.All.SendAsync(HubConstants.OnRaceInit, race);
            _buffer.Initialize(race, racers);
        }
    }
}
