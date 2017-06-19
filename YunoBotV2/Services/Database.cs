using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Collections.Concurrent;
using Discord.WebSocket;
using YunoBotV2.Core;

namespace YunoBotV2.Services
{
    public static class Database
    {

        private static ConcurrentDictionary<ulong, GuildSetting> _guildSettings;
        private const string DbPath = "Data Source = Database/Database.db";
        private const string DefaultPrefix = "e$";

        public static async Task SavePrefix(ulong id, string prefix)
        {

            var setting = _guildSettings[id];
            setting.Prefix = prefix;

            using (var connection = new SqliteConnection(DbPath))
            {

                await connection.OpenAsync();

                await connection.UpdateAsync(setting);

                connection.Close();

            }

        }

        public static bool GetPrefix(ulong id, out string prefix)
        {

            if (!_guildSettings.ContainsKey(id))
            {

                prefix = null;
                return false;

            }
            else
            {

                prefix = _guildSettings[id].Prefix;
                return true;

            }

        }

        public static bool IsSelfRole(SocketRole role)
        {

            var setting = _guildSettings[role.Guild.Id];

            if (setting.SelfRoles.Contains(role.Id))
                return true;
            else
                return false;

        }

        public static ulong[] GetRoles(SocketGuild guild)
        {

            var setting = _guildSettings[guild.Id];

            return setting.SelfRoles;

        }

        public static async Task AddSelfRole(SocketRole role)
        {

            var setting = _guildSettings[role.Guild.Id];
            setting.AddSelfRole(role.Id);

            using (var connection = new SqliteConnection(DbPath))
            {

                await connection.OpenAsync();

                await connection.UpdateAsync(setting);

                connection.Close();

            }

        }

        public static async Task RemoveSelfRole(SocketRole role)
        {

            var setting = _guildSettings[role.Guild.Id];
            setting.RemoveSelfRole(role.Id);

            using (var connection = new SqliteConnection(DbPath))
            {

                await connection.OpenAsync();

                await connection.UpdateAsync(setting);

                connection.Close();

            }
            
        }

        public static async Task ExecuteSql(string statement)
        {

            using (var connection = new SqliteConnection(DbPath))
            {

                await connection.OpenAsync();
                
                await connection.ExecuteAsync(statement);

                connection.Close();

            }

        }

        public static async Task CreateSettings(SocketGuild guild)
        {

            using (var connection = new SqliteConnection(DbPath))
            {

                await connection.OpenAsync();

                var possibleEntry = await connection.ExecuteScalarAsync<ulong>("select Id from GuildSettings where Id = @Id", new { Id = guild.Id });

                if (possibleEntry == 0)
                {

                    var setting = new GuildSetting
                    {
                        Id = guild.Id,
                        Prefix = DefaultPrefix,
                    };

                    await connection.InsertAsync(setting);

                    _guildSettings[guild.Id] = setting;

                }

                connection.Close();

            }

        }

        public static async Task InitializeSettings(DiscordSocketClient client)
        {

            Log("Initializing settigns...");

            using (var connection = new SqliteConnection(DbPath))
            {

                await connection.OpenAsync();

                var settings = await connection.GetAllAsync<GuildSetting>();
                _guildSettings = new ConcurrentDictionary<ulong, GuildSetting>(settings.ToDictionary(setting => setting.Id, setting => setting));
                var uninitializedGuilds = client.Guilds.Where(guild => !_guildSettings.ContainsKey(guild.Id)).ToList();

                foreach (var guild in uninitializedGuilds)
                {

                    await CreateSettings(guild);

                }

                connection.Close();

            }

            Log("Finished initializing settings.");

        }

        private static void Log(string message)
            => AltConsole.Print("Service", "Database", message);

    }
    
    [Table("GuildSettings")]
    public class GuildSetting
    {

        [ExplicitKey]
        public ulong Id { get; set; }
        public string Prefix { get; set; }
        [Computed, Write(false)]
        public ulong[] SelfRoles { get { return SelfRoleString.Split('/').Select(str => ulong.Parse(str)).ToArray(); } }

        private string SelfRoleString { get; set; } = "0";

        public GuildSetting()
        {

            //SelfRoleString = "0";

        }

        public void AddSelfRole(ulong id)
        {

            if (SelfRoleString == "0")
                SelfRoleString = SelfRoleString.Replace("0", $"{id}");
            else
                SelfRoleString += $"/{id}";

        }

        public void RemoveSelfRole(ulong id)
        {

            SelfRoleString = SelfRoleString.Replace(id.ToString(), string.Empty).Replace("//", "/");

            if (SelfRoleString.EndsWith("/"))
                SelfRoleString = SelfRoleString.Substring(0, SelfRoleString.Length - 1);

            if (string.IsNullOrEmpty(SelfRoleString))
                SelfRoleString = "0";

        }

    }
}
