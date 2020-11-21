using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using Core;
using Dapper;
using Newtonsoft.Json;

namespace RaceFiller
{
    class Program
    {
        private static readonly string BaseUrl = "https://localhost:44379/";
        private static Random Rnd { get; } = new Random(2020);

        static void Main(string[] args)
        {
            var race = new Race
            {
                Id = 1,
                Name = "Race 1 from console",
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
            return;
            //var racers = FillRacersInfo(20);
            //FillSeasonsInfo(racers, 4, 25, 500);
        }

        private static void Initialize(Race race, Racer[] racers)
        {
            var url = BaseUrl + "init";
            var contentString = $"{JsonConvert.SerializeObject(race)}&{JsonConvert.SerializeObject(racers)}";

            using var client = new HttpClient();
            using var res = client.PutAsync(url, new StringContent(contentString)).Result;
            using var content = res.Content;

            var data = content.ReadAsStringAsync().Result;
            if (data != null)
            {
                Console.WriteLine(data);
            }
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
