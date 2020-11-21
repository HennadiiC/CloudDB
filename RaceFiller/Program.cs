using System;
using System.Linq;
using System.Net.Http;
using Core;
using Newtonsoft.Json;

namespace RaceFiller
{
    class Program
    {
        private static readonly string BaseUrl = "https://localhost:44379/";
        //private static readonly string BaseUrl = "https://racing.azurewebsites.net/";

        private static Random Rnd { get; } = new Random(2020);

        private static QueueService QueueService { get; } = new QueueService();

        static void Main(string[] args)
        {
            var race = new Race
            {
                Id = 1,
                Name = "Race 1 from console 2",
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

            Initialize(race, racers);

            var cpp = new CheckpointPassing
            {
                Id = 6,
                RacerId = racers[0].Id,
                CheckpointId = race.Checkpoints[0].Id,
                Time = DateTime.Now,
            };

            AddCheckpointPassing(cpp);
        }

        private static void Initialize(Race race, Racer[] racers)
        {
            var url = BaseUrl + "init";
            var contentString = $"{JsonConvert.SerializeObject(race)}&{JsonConvert.SerializeObject(racers)}";

            using var client = new HttpClient();
            using var res = client.PutAsync(url, new StringContent(contentString)).Result;
        }

        private static void AddCheckpointPassing(CheckpointPassing checkpointPassing)
        {
            QueueService.SendMessage(JsonConvert.SerializeObject(checkpointPassing));
        }

        static int[] FillRacersInfo(int racersCount)
        {
            var racerIds = Enumerable.Range(1, racersCount)
                .AsParallel()
                .ToArray();

            return racerIds;
        }

        static void FillSeasonsInfo(int[] racerIds, int seasonsCount, int racesInOneSeason, int checkpointsInOneRace)
        {
            Enumerable.Range(1, seasonsCount)
                .AsParallel()
                .Select(sn =>
                {
                    FillOneSeason(racerIds, racesInOneSeason, checkpointsInOneRace);
                    return 0;
                })
                .ToArray();
        }

        private static void FillOneSeason(int[] racerIds, int racesInOneSeason, int checkpointsInOneRace)
        {
            Enumerable.Range(1, racesInOneSeason)
                .AsParallel()
                .Select(rn =>
                {
                    
                    return $"Race {rn} in season";
                })
                .ToArray();
        }


        static void FillOneRacing(string racingName, int checkPointsCount, int[] racerIds, int season)
        {
            var checkpointIds = Enumerable.Range(1, checkPointsCount)
                .AsParallel()
                .Select(i => ($"CP {i}"))
                .OrderBy(id => id)
                .ToArray();

        }

        static void FillCheckpointPassings(int[] checkpointIds, int[] racerIds)
        {
            var now = DateTime.Now;

            racerIds.AsParallel().ForAll(racerId =>
            {
                var passTime = now.AddSeconds(GetRndInt(0, 30));

                foreach (var checkpointId in checkpointIds)
                {
                    passTime = passTime.AddSeconds(GetRndInt(0, 1200));

                }
            });
        }


        #region Utils
        private static int GetRndInt(int minValue, int maxValue)
        {
            return Rnd.Next(minValue, maxValue);
        }

        #endregion
    }
}
