using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dapper;

namespace RaceFiller
{
    class Program
    {
        private static readonly string _connectionString = @"Server=tcp:tsi-chm.database.windows.net,1433;Initial Catalog=races;Persist Security Info=False;User ID=user-admin;Password=DifficultPass1!;MultipleActiveResultSets=False;TrustServerCertificate=False;";
        
        private static readonly object Lock = new object();

        private static Random Rnd { get; } = new Random(2020);

        static void Main(string[] args)
        {
            return;
            var racers = FillRacersInfo(20);
            FillSeasonsInfo(racers, 4, 25, 500);
        }

        static int[] FillRacersInfo(int racersCount)
        {
            var countries = Enumerable.Range(1, racersCount / 2 + 1)
                .AsParallel()
                .Select(n => AddCountry($"Country {n}"))
                .ToArray();

            var racerIds = Enumerable.Range(1, racersCount)
                .AsParallel()
                .Select(i =>
                {
                    var countryIndex = GetRndInt(0, countries.Length);
                    return AddRacer($"Racer {i}", $"{(char) ('A' + i - 1)}", countries[countryIndex]);
                })
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
            var season = AddSeason("Season Name");

            Enumerable.Range(1, racesInOneSeason)
                .AsParallel()
                .Select(rn =>
                {
                    FillOneRacing($"Race {rn} in season {season}", checkpointsInOneRace, racerIds, season);
                    return 0;
                })
                .ToArray();
        }


        static void FillOneRacing(string racingName, int checkPointsCount, int[] racerIds, int season)
        {
            var racingId = AddRacing(season, racingName);

            var checkpointIds = Enumerable.Range(1, checkPointsCount)
                .AsParallel()
                .Select(i => AddCheckPoint($"CP {i}", i == 1 ? 0 : (float)GetRndDouble() * 100, racingId))
                .OrderBy(id => id)
                .ToArray();

            FillCheckpointPassings(checkpointIds, racerIds);
        }

        static void FillCheckpointPassings(int[] checkpointIds, int[] racerIds)
        {
            var now = DateTime.Now;

            racerIds.AsParallel().ForAll(racerId =>
            {
                var passTime = now.AddSeconds(GetRndInt(0, 30));

                foreach (var checkpointId in checkpointIds)
                {
                    var sql = $@"EXEC	[dbo].[SaveCheckpointPassing]
		                                    @checkpointId = {checkpointId},
		                                    @racerId = {racerId},
		                                    @passTime = '{passTime}'
                                    ";

                    passTime = passTime.AddSeconds(GetRndInt(0, 1200));

                    using var db = GetConnection();
                    db.Execute(sql, commandTimeout: 0);
                }
            });
        }



        #region Table Inserts

        static int AddCountry(string name)
        {
            var sql = @$"INSERT INTO [dbo].[Countries] ([Name])
                                VALUES ('{name}')
                                select SCOPE_IDENTITY()";
            using var db = new SqlConnection(_connectionString);
            return db.ExecuteScalar<int>(sql, commandTimeout:0);
        }
        static int AddRacer(string fName, string lName, int countryId)
        {
            var sql = @$"INSERT INTO [dbo].[Racers] ([FirstName], [LastName], [CountryId])
                                VALUES ('{fName}', '{lName}', {countryId})
                                select SCOPE_IDENTITY()";

            using var db = new SqlConnection(_connectionString);
            return db.ExecuteScalar<int>(sql, commandTimeout: 0);
        }


        static int AddSeason(string seasonName)
        {
            var sql = @$"INSERT INTO [dbo].[Seasons] ([Name])
                                VALUES ('{seasonName}')
                            select SCOPE_IDENTITY()";
            using var db = new SqlConnection(_connectionString);
            return db.ExecuteScalar<int>(sql, commandTimeout: 0);
        }

        static int AddRacing(int season, string racingName)
        {
            var sql = $@"INSERT INTO [dbo].[Racings] ([Name],[SeasonId]) 
                                VALUES ('{racingName}', {season})
                            select SCOPE_IDENTITY()";
            using var db = new SqlConnection(_connectionString);
            return db.ExecuteScalar<int>(sql, commandTimeout: 0);
        }

        static int AddCheckPoint(string checkpointName, float distance, int racingId)
        {
            var sql = $@"INSERT INTO [dbo].[Checkpoints]
                               ([Name]
                               ,[Distance]
                               ,[RaceId])
                         VALUES
                               ('{checkpointName}'
                               ,{distance}
                               ,{racingId})

                            select SCOPE_IDENTITY()";

            using var db = new SqlConnection(_connectionString);
            return db.ExecuteScalar<int>(sql, commandTimeout: 0);
        }

        #endregion

        #region Utils
        private static int GetRndInt(int minValue, int maxValue)
        {
            int res;
            lock (Lock)
            {
                res = Rnd.Next(minValue, maxValue);
            }

            return res;
        }
        private static double GetRndDouble()
        {
            double nextDouble;
            lock (Lock)
            {
                nextDouble = Rnd.NextDouble();
            }
            return nextDouble;
        }

        private static SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        #endregion
    }
}
