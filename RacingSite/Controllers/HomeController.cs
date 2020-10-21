using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RacingSite.Models;

namespace RacingSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _connection;

        public HomeController(IConfiguration config)
        {
            _connection = config.GetValue<string>("connectionString");
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Racers()
        {
            var racers = GetRacers();
            return View(racers);
        }

        private List<Racer> GetRacers()
        {
            var racers = GetTable<Racer>("Racers");
            return racers;
        }

        public IActionResult Checkpoints()
        {
            var checkPoints = GetCheckPoints();
            return View(checkPoints);
        }

        private List<CheckPoint> GetCheckPoints()
        {
            return GetTable<CheckPoint>("Checkpoints").OrderBy(c => int.Parse(c.Name.Split(" ").Last())).ToList();
        }

        public IActionResult CheckpointPasses()
        {
            var passes = GetTable<CheckpointPass>("CheckpointPasses", "top 100");
            return View(passes);
        }

        private List<T> GetTable<T>(string tableName, string top = null)
        {
            using var db = new SqlConnection(_connection);
            return db.Query<T>($"select {top} * from {tableName}").ToList();
        }

        [HttpGet]
        public IActionResult PassedDistance([FromQuery] int id = 2)
        {
            var distance = ExecuteSp<float>("GetPassedDistanceForRacer",
                new
                {
                    racerId = id,
                    raceId = 2
                })[0];

            var model = new PassedDistanceViewModel
            {
                SelectedRacer = id,
                Racers = GetRacers(),
                Distance = distance
            };
            return View(model);
        }

        private List<T> ExecuteSp<T>(string spName, object p)
        {
            using var db = new SqlConnection(_connection);
            return db.Query<T>(spName, p, commandType: CommandType.StoredProcedure).ToList();
        }

        public IActionResult RacerSpeed([FromQuery] RacerSpeedFilter filter)
        {
            if (filter.RacerId == 0)
            {
                filter.RacerId = filter.CheckpointId = 3;
            }
            var cps = GetCheckPoints().Where(c => c.Distance > 0).ToList();
            var racers = GetRacers();
            
            var speed = 0D;
            try
            {
                speed = ExecuteSp<double>("GetRacerSpeedForCheckPoint", filter)[0];
            }
            catch
            {

            }
            var m = new RacerSpeed
            {
                Racers = racers,
                CheckPoints = cps,
                Speed = speed,
                SelectedRacer = filter.RacerId,
                SelectedCheckPoint = filter.CheckpointId
            };

            return View(m);
        }

        public IActionResult RacersRating([FromQuery] int checkpointId)
        {
            if (checkpointId == 0)
            {
                checkpointId = 2;
            }
            var rating = ExecuteSp<CheckPointPassStats>("GetRacersRatingForCheckpoint",
                new { checkpointId });

            var m = new RacersRatingViewModel
            {
                SelectedCheckPoint = checkpointId,
                Rating = rating,
                CheckPoints = GetCheckPoints()
            };
            return View(m);
        }

        public IActionResult PassedTime([FromQuery] PassedTimeFilter filter)
        {
            if (filter.RacerId == 0)
            {
                filter.RacerId = filter.CheckpointId = 2;
            }
            var passedTimeInMinutes = ExecuteSp<long>("GetPassedTimeOfRacer", filter)[0];

            var m = new PassedTimeViewModel
            {
                Filter = filter,
                PassedTimeInMinutes = passedTimeInMinutes,
                Checkpoints = GetCheckPoints(),
                Racers = GetRacers()
            };
            return View(m);
        }
    }
}
