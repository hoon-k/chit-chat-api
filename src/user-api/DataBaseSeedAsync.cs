using System;
using System.Threading.Tasks;
using Dapper;
using Npgsql;

namespace ChitChatAPI.UserAPI
{
    public class DataBaseSeedAsync 
    {
        public static async Task SeedAsync(string connectionString) 
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                await CreateDataBase(connection);
                // await CreateAccountTable(connection);
                // await CreateProfileTable(connection);
            }
        }

        private static async Task CreateDataBase(NpgsqlConnection cnx)
        {
            var sql = @"
                CREATE DATABASE chitchat_users
                    WITH 
                    OWNER = postgres
                    ENCODING = 'UTF8'
                    LC_COLLATE = 'C'
                    LC_CTYPE = 'C'
                    TABLESPACE = pg_default
                    CONNECTION LIMIT = -1;
            ";

            using (var cmd = new NpgsqlCommand())
            {
                cmd.Connection = cnx;
                cmd.CommandText = sql;
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}