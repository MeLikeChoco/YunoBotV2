﻿using Discord.WebSocket;
using Discord.Commands;
using Discord;
using WS4NetCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using YunoBotV2.Configuration;
using YunoBotV2.Services.WebServices;
using YunoBotV2.Services;
using Microsoft.Extensions.DependencyInjection;

namespace YunoBotV2.Core
{
    public class Program
    {

        public static void Main(string[] args)
            => new Program().Run(args).GetAwaiter().GetResult();

        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _serviceProvider;

        private int _latencyLimiter = 20;
        private bool IsTest = false;

        private string[] Args;

        public async Task Run(string[] args)
        {

            Args = args;
            ParseArgs();
            Config.LoadConfig();
            AltConsole.Print("Program", "Client", "Initializing...");

            _client = new DiscordSocketClient(new DiscordSocketConfig
            {

                AlwaysDownloadUsers = true,
                LogLevel = LogSeverity.Verbose,
                MessageCacheSize = 1000,
                WebSocketProvider = WS4NetProvider.Instance,                

            });
            _commands = new CommandService(new CommandServiceConfig
            {

                CaseSensitiveCommands = false,
                LogLevel = LogSeverity.Verbose,
                DefaultRunMode = RunMode.Async,

            });

            Log();
            await RegisterCommands();
            await StartServices();

            await LoginAndConnect();

            SetGame();
            ForceReconnect();

            await Task.Delay(-1);

        }

        public async Task StartServices()
        {

            var web = new Web();

            _serviceProvider = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(web)
                .AddSingleton<Unshortener>()
                .AddSingleton<Zalgo>()
                .BuildServiceProvider();

            await Cache.InitializeCache(web);

            _client.JoinedGuild += Database.CreateSettings;
            _client.Ready += async () => { await Database.InitializeSettings(_client); };
            _client.UserJoined += Database.AssignAutoRole;

        }

        public void Log()
        {

            _commands.Log += (message)
                => Task.Run(()
                => AltConsole.Print(message.Severity, message.Source, message.Message, message.Exception));

            _client.Log += (message)
                => Task.Run(()
                => AltConsole.Print(message.Severity, message.Source, message.Message, message.Exception));

        }

        public void SetGame()
        {

            _client.Ready += ()
                => Task.Run(async ()
                =>
                {
                    AltConsole.Print("Program", "Client", "Setting game...");
                    await _client.SetGameAsync(Config.Game);
                    AltConsole.Print("Program", "Client", "Setting game...");
                });

        }

        public async Task RegisterCommands()
        {

            _client.MessageReceived += CommandHandler;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
            var test = _commands.Modules;

        }

        public async Task CommandHandler(SocketMessage possibleCmd)
        {

            var message = possibleCmd as SocketUserMessage;
            ulong guildId;

            if (message.Channel is IDMChannel) guildId = 1;
            else guildId = (message.Channel as SocketGuildChannel).Guild.Id;

            //var guildId = (message.Channel as SocketGuildChannel).Guild.Id;

            if (message == null) return;
            if (message.Author.IsBot) return;

            var argPos = 0;

            if (Database.GetPrefix(guildId, out var prefix)) { }
            else prefix = "e$";

            if ((message.HasStringPrefix(prefix, ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos)) 
                && !message.Content.Trim().Equals(prefix))
            {

                var context = new SocketCommandContext(_client, message);
                AltConsole.Print("Verbose", "Command", $"{(message.Channel as SocketGuildChannel).Guild.Name}");
                AltConsole.Print("Verbose", "Command", $"{message.Content}");

                var result = await _commands.ExecuteAsync(context, argPos, _serviceProvider);

                if (!result.IsSuccess)
                {

                    if (result.ErrorReason.ToLower().Contains("unknown command"))
                        return;

                    await context.Channel.SendMessageAsync("There was an error in the command.");
                    //await context.Channel.SendMessageAsync("https://goo.gl/JieFJM");

                    AltConsole.Print("Error", "Error", result.ErrorReason);
                    //debug purposes
                    //await context.Channel.SendMessageAsync($"**Error:** {result.ErrorReason}");

                }

            }

        }

        public async Task LoginAndConnect()
        {

            string token = IsTest ? Config.Test : Config.Token;

            AltConsole.Print("Client", "Gateway", "Logging in...");
            await _client.LoginAsync(TokenType.Bot, token);
            AltConsole.Print("Client", "Gateway", "Logged in.");
            AltConsole.Print("Client", "Gateway", "Starting up...");
            await _client.StartAsync();
            AltConsole.Print("Client", "Gateway", "Finished starting up...");

        }

        public void ForceReconnect()
        {

            _client.Disconnected += async (ex) =>
            {

                await Task.Delay(10000);

                if (!_client.ConnectionState.Equals(ConnectionState.Connected))
                {
                    AltConsole.Print(LogSeverity.Warning, "Client", "Reconnecting...");
                    await Run(Args);
                }

            };

        }

        public void ParseArgs()
        {

            if (Args.Contains("--test"))
            {
                if (bool.TryParse(Args.ElementAt(Args.ToList().IndexOf("--test") + 1), out bool parseTest)) IsTest = parseTest;
            }

        }

    }
}
