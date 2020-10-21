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
        private static readonly string _connectionString = @"Server=tcp:tsi-chm.database.windows.net,1433;Initial Catalog=racing;Persist Security Info=False;User ID=user-admin;Password=DifficultPass1!;MultipleActiveResultSets=False;TrustServerCertificate=False;";

        static void Main(string[] args)
        {
            FillRacing("Racing X", 1000, 1000);
        }

        static void FillRacing(string racingName, int checkPointsCount, int racersCount)
        {
            var rnd = new Random(2020);

            var racingId = AddRacing(racingName);

            var checkpointIds = Enumerable.Range(1, checkPointsCount)
                .AsParallel()
                .Select(i => AddCheckPoint($"CP {i}", i == 1 ? 0 : (float)rnd.NextDouble() * 100, racingId))
                .ToArray()
                .OrderBy(id => id)
                .ToArray();

            var racerIds = Enumerable.Range(1, racersCount)
                .AsParallel()
                .Select(i => AddRacer($"Racer {(char)('A' + i - 1)}"))
                .ToArray()
                .OrderBy(id => id)
                .ToArray(); ;

            FillCheckpointPassings(checkpointIds, racerIds, rnd);
        }

        static int AddRacing(string racingName)
        {
            var sql = $@"INSERT INTO [dbo].[Racings] ([Name]) 
                                VALUES ('{racingName}')
                            select SCOPE_IDENTITY()";
            using var db = new SqlConnection(_connectionString);
            return db.ExecuteScalar<int>(sql);
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
            return db.ExecuteScalar<int>(sql);
        }

        static int AddRacer(string name)
        {
            var sql = @$"INSERT INTO [dbo].[Racers] ([FullName])
                                VALUES ('{name}')
                                select SCOPE_IDENTITY()";

            using var db = new SqlConnection(_connectionString);
            return db.ExecuteScalar<int>(sql);
        }

        static void FillCheckpointPassings(int[] checkpointIds, int[] racerIds, Random rnd)
        {
            var now = DateTime.Now;

            racerIds.AsParallel().ForAll(racerId =>
            {
                var passTime = now.AddSeconds(rnd.Next(0, 30));
                var sb = new StringBuilder();

                foreach (var checkpointId in checkpointIds)
                {

                    var sql = $@"EXEC	[dbo].[SaveCheckpointPassing]
		                                    @checkpointId = {checkpointId},
		                                    @racerId = {racerId},
		                                    @passTime = '{passTime}'
                                    ";

                    sb.Append(sql).Append("\n\n");

                    passTime = passTime.AddSeconds(rnd.Next(0, 1200));
                }

                using var db = GetConnection();
                db.Execute(sb.ToString());
            });
        }

        private static SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
