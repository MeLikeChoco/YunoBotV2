using Discord.WebSocket;
using Discord.Commands;
using Discord;
using WS4NetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Reflection;
using YunoBotV2.Configuration;
using YunoBotV2.Services.WebServices;
using YunoBotV2.Services;

namespace YunoBotV2.Core
{
    public class Program
    {

        public static void Main(string[] args)
            => new Program().Run(args).GetAwaiter().GetResult();

        internal DiscordSocketClient _client;
        internal CommandService _commands;
        internal DependencyMap _map;

        internal int _latencyLimiter = 20;
        internal bool IsTest = false;

        internal string[] Args;

        internal async Task Run(string[] args)
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
            InjectServices();

            await LoginAndConnect();

            SetGame();
            ForceReconnect();

            await Task.Delay(-1);

        }

        internal void InjectServices()
        {
            
            _map = new DependencyMap();
            _map.Add(_client);
            _map.Add(new Web());
            _map.Add(new Unshortener());
            _map.Add(new Zalgo());

        }

        internal void Log()
        {

            _commands.Log += (message)
                => Task.Run(()
                =>
                {

                    if (string.IsNullOrEmpty(message.Message))
                        return;
                    if (message.Message.Contains("Latency"))
                    {

                        if (_latencyLimiter == 20)
                        {
                            AltConsole.Print(message.Severity, message.Source, message.Message, message.Exception);
                            _latencyLimiter = 0;
                        }
                        else _latencyLimiter++;

                    }
                    else AltConsole.Print(message.Severity, message.Source, message.Message, message.Exception);

                });

            _client.Log += (message)
                => Task.Run(()
                =>
                {
                    if (string.IsNullOrEmpty(message.Message))
                        return;
                    if (message.Message.Contains("Latency"))
                    {

                        if (_latencyLimiter == 20)
                        {
                            AltConsole.Print(message.Severity, message.Source, message.Message, message.Exception);
                            _latencyLimiter = 0;
                        }
                        else _latencyLimiter++;

                    }
                    else AltConsole.Print(message.Severity, message.Source, message.Message, message.Exception);
                });

        }

        internal void SetGame()
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

        internal async Task RegisterCommands()
        {

            _client.MessageReceived += CommandHandler;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly());

        }

        internal async Task CommandHandler(SocketMessage possibleCmd)
        {

            var message = possibleCmd as SocketUserMessage;
            ulong guildId;

            if (message.Channel is IDMChannel) guildId = 1;
            else guildId = (message.Channel as SocketGuildChannel).Guild.Id;

            //var guildId = (message.Channel as SocketGuildChannel).Guild.Id;

            if (message == null) return;
            if (message.Author.IsBot) return;

            var argPos = 0;
            var prefix = "e$";

            //if (_guildService._guildPrefixes.TryGetValue(guildId, out string prefix)) { }
            //else prefix = "y!";

            if (!(message.HasStringPrefix(prefix, ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos)))
                return;
            if (message.Content.Trim().Equals(prefix))
                return;

            var context = new SocketCommandContext(_client, message);
            AltConsole.Print("Verbose", "Command", $"{(message.Channel as SocketGuildChannel).Guild.Name}");
            AltConsole.Print("Verbose", "Command", $"{message.Content}");
            IResult result = await _commands.ExecuteAsync(context, argPos, _map);

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

        internal async Task LoginAndConnect()
        {

            string token = IsTest ? Config.Test : Config.Token;

            AltConsole.Print("Client", "Gateway", "Logging in...");
            await _client.LoginAsync(TokenType.Bot, token);
            AltConsole.Print("Client", "Gateway", "Logged in.");
            AltConsole.Print("Client", "Gateway", "Starting up...");
            await _client.StartAsync();
            AltConsole.Print("Client", "Gateway", "Finished starting up...");

        }

        internal void ForceReconnect()
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

        internal void ParseArgs()
        {
            
            if (Args.Contains("--test"))
            {
                if (bool.TryParse(Args.ElementAt(Args.ToList().IndexOf("--test") + 1), out bool parseTest)) IsTest = parseTest;
            }

        }

    }
}
