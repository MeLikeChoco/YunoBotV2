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
using YunoBotV2.Objects.Deserializers;
using Microsoft.Data.Sqlite;

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

        public static async Task AddAutoRole(SocketRole role)
        {

            var setting = _guildSettings[role.Guild.Id];
            setting.AddAutoRole(role.Id);

            using (var connection = new SqliteConnection(DbPath))
            {

                await connection.OpenAsync();

                await connection.UpdateAsync(setting);

                connection.Close();

            }

        }

        public static async Task RemoveAutoRole(SocketRole role)
        {

            var setting = _guildSettings[role.Guild.Id];
            setting.RemoveAutoRole(role.Id);

            using (var connection = new SqliteConnection(DbPath))
            {

                await connection.OpenAsync();

                await connection.UpdateAsync(setting);

                connection.Close();

            }

        }

        public static ulong[] GetAutoRoles(SocketGuild guild)
        {

            var setting = _guildSettings[guild.Id];

            return setting.AutoRoles;

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

        public static async Task AssignAutoRole(SocketGuildUser user)
        {

            var guild = user.Guild;
            var setting = _guildSettings[guild.Id];
            var roles = setting.AutoRoles.Select(u => guild.GetRole(u));

            if(roles.Count() != 0)
                await user.AddRolesAsync(roles);

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

            Log("Initializing settings...");

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
}
