using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YunoV3.Objects;

namespace YunoV3.Core
{
    public class Program
    {

        static async Task Main(string[] args)
        {

            Logger.Initialize();
            Logger.Log("YunoBot is up and running! Ready to kill for love and attention!");

            var client = new DiscordSocketClient(new DiscordSocketConfig
            {

                LogLevel = LogSeverity.Verbose,
                MessageCacheSize = 1000

            });

            var cmdService = new CommandService(new CommandServiceConfig
            {

                LogLevel = LogSeverity.Verbose,
                CaseSensitiveCommands = false,
                DefaultRunMode = RunMode.Async //the overhead is nothing to be worried about

            });

            var tokens = new Tokens();
            var settings = new BotSettings();
            var events = new Events(client, cmdService);

            client.Log += events.Log;
            cmdService.Log += events.Log;
            client.Ready += events.ReadyHerWeapons;
            client.JoinedGuild += events.GenGuildSetting;
            client.UserJoined += events.GiveAutoRoles;

            var token = settings.IsTest ? tokens.DiscordTest : tokens.DiscordLegit;

            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            await Task.Delay(-1);

        }

    }
}
