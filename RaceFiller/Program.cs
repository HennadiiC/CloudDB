using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Core;
using Newtonsoft.Json;

namespace RaceFiller
{
    class Program
    {
        private static readonly string BaseUrl = 0 == 1 ? "https://localhost:44379/" : "https://racing.azurewebsites.net/";

        private static Random Rnd { get; } = new Random(2020);

        private static QueueService QueueService { get; } = new QueueService();

        static void Main(string[] args)
        {
            Start(6, 5, 2, 3);
        }

        private static void Start(int checkpointsCount, int racersCount, int perIntervalCount, int intervalInSeconds)
        {
            var race = new Race
            {
                Id = 1,
                Name = $"Race {DateTime.Now:T}",
                Checkpoints = Enumerable.Range(1, checkpointsCount)
                    .Select(i => new Checkpoint { Id = i, Name = $"Checkpoint #{i}" })
                    .ToArray()
            };

            var racers = Enumerable.Range(1, racersCount)
                .Select(i => new Racer { Id = i, Name = $"Racer {i}" }).ToArray();

            Initialize(race, racers);

            Console.ReadKey();

            var groups = Enumerable.Range(1, racersCount)
                .GroupBy(i => (i - 1) / perIntervalCount)
                .Select(g => g.ToArray())
                .ToArray();

            var tasks = new List<Task>();

            foreach (var racersInGroup in groups)
            {
                foreach (var id in racersInGroup)
                {
                    var racerId = id;
                    var t = Task.Run(() =>
                    {
                        foreach (var checkpoint in race.Checkpoints)
                        {
                            AddCheckpointPassing(new CheckpointPassing
                            {
                                CheckpointId = checkpoint.Id,
                                RacerId = racerId,
                                Id = 0,
                                Time = DateTime.Now,
                            });
                            Thread.Sleep(Rnd.Next(0, 10) * 1000);
                        }
                    });
                    tasks.Add(t);
                }
                Thread.Sleep(intervalInSeconds * 1000);
            }

            Task.WaitAll(tasks.ToArray());
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
