using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;

namespace YunoBotV2.Services
{
    public static class Database
    {

        private const string DbPath = "Data Source = Database/Database.db";

        public static async Task SavePrefix(ulong id, string prefix)
        {

            var key = new IdAndPrefix { Id = id, Prefix = prefix };

            using (var connection = new SqliteConnection(DbPath))
            {

                await connection.OpenAsync();

                if ((await connection.ExecuteScalarAsync<ulong>("select Id from Prefixes where Id = @Id", key)) != 0) //check if exists
                {
                    await connection.UpdateAsync(key);
                }
                else
                {
                    await connection.InsertAsync(key);
                }

                connection.Close();

            }

            Cache.Prefixes.AddOrUpdate(id, prefix, (oldId, oldPrefix) => oldPrefix = prefix);

        }

        public static async Task<Dictionary<ulong, string>> GetPrefixes()
        {

            Dictionary<ulong, string> dict;

            using (var connection = new SqliteConnection(DbPath))
            {

                await connection.OpenAsync();

                dict = connection.QueryAsync("select * from Prefixes").Result.ToDictionary(column => (ulong)column.Id, column => (string)column.Prefix);

                connection.Close();

            }

            return dict;

        }

    }

    [Table("Prefixes")]
    class IdAndPrefix
    {

        [ExplicitKey]
        public ulong Id { get; set; }
        public string Prefix { get; set; }

    }
}
