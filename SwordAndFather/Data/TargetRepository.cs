using System;
using System.Data.SqlClient;
using Dapper;
using SwordAndFather.Models;

namespace SwordAndFather.Data
{
    public class TargetRepository
    {
        const string ConnectionString = "Server = localhost; Database = SwordAndFather; Trusted_Connection = True;";

        public Target AddTarget(string location, string name, FitnessLevel fitnessLevel, int userId)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var insertQuery = @"
                    INSERT INTO[dbo].[Targets]
                           ([Location]
                           ,[Name]
                           ,[FitnessLevel]
                           ,[UserId])
                    output inserted.*
                    VALUES
                           (@location
                           ,@name
                           ,@fitnessLevel
                           ,@userId)";

                var parameters = new
                {
                    Location = location,
                    Name = name,
                    FitnessLevel = fitnessLevel,
                    UserId = userId
                };

                var newTarget = db.QueryFirstOrDefault<Target>(insertQuery, parameters);

                if (newTarget != null)
                {
                    return newTarget;
                }
                throw new Exception("Could not create new target");
            }
        }
    }
}
