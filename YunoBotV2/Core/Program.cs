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

namespace YunoBotV2.Core
{
    public class Program
    {

        public static void Main(string[] args)
            => new Program().Run(args).GetAwaiter().GetResult();

        private DiscordSocketClient _client;
        private CommandService _commands;
        private DependencyMap _map;
        private int LatencyLimiter;
        private bool IsTest = false;
        private string[] Args;

        public async Task Run(string[] args)
        {

            Args = args;
            ParseArgs();
            Config.LoadConfig();
            AltConsole.Print("Program", "Client", "Initializing...");

            LatencyLimiter = 20;
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

            });

            Log();
            await RegisterCommands();
            InjectServices();

            await LoginAndConnect();

            SetGame();
            ForceReconnect();

            await Task.Delay(-1);

        }

        private void InjectServices()
        {
            
            _map = new DependencyMap();
            _map.Add(_client);
            _map.Add(new Web());
            _map.Add(new Unshortener());

        }

        private void Log()
        {

            _commands.Log += (message)
                => Task.Run(()
                =>
                {

                    if (string.IsNullOrEmpty(message.Message)) return;
                    if (message.Message.Contains("Latency"))
                    {

                        if (LatencyLimiter == 20)
                        {
                            AltConsole.Print(message.Severity, message.Source, message.Message, message.Exception);
                            LatencyLimiter = 0;
                        }
                        else LatencyLimiter++;

                    }
                    else AltConsole.Print(message.Severity, message.Source, message.Message, message.Exception);

                });

            _client.Log += (message)
                => Task.Run(()
                =>
                {
                    if (string.IsNullOrEmpty(message.Message)) return;
                    if (message.Message.Contains("Latency"))
                    {

                        if (LatencyLimiter == 20)
                        {
                            AltConsole.Print(message.Severity, message.Source, message.Message, message.Exception);
                            LatencyLimiter = 0;
                        }
                        else LatencyLimiter++;

                    }
                    else AltConsole.Print(message.Severity, message.Source, message.Message, message.Exception);
                });

        }

        private void SetGame()
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

        private async Task RegisterCommands()
        {

            _client.MessageReceived += CommandHandler;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly());

        }

        private async Task CommandHandler(SocketMessage possibleCmd)
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

            if (!(message.HasStringPrefix(prefix, ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos))) return;

            var context = new SocketCommandContext(_client, message);
            AltConsole.Print("Verbose", "Command", $"{(message.Channel as SocketGuildChannel).Guild.Name}");
            AltConsole.Print("Verbose", "Command", $"{message.Content}");
            IResult result = await _commands.ExecuteAsync(context, argPos, _map);

            if (!result.IsSuccess)
            {

                await context.Channel.SendMessageAsync("It seems you have encountered the shadow realm, command used wrong.");
                //await context.Channel.SendMessageAsync("https://goo.gl/JieFJM");

                AltConsole.Print("Error", "Error", result.ErrorReason);

                //debug purposes
                //await context.Channel.SendMessageAsync($"**Error:** {result.ErrorReason}");

            }

        }

        private async Task LoginAndConnect()
        {

            string token = IsTest ? Config.Test : Config.Token;

            AltConsole.Print("Client", "Gateway", "Logging in...");
            await _client.LoginAsync(TokenType.Bot, token);
            AltConsole.Print("Client", "Gateway", "Logged in.");
            AltConsole.Print("Client", "Gateway", "Starting up...");
            await _client.StartAsync();
            AltConsole.Print("Client", "Gateway", "Finished starting up...");

        }

        private void ForceReconnect()
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

        private void ParseArgs()
        {
            
            if (Args.Contains("--test"))
            {
                if (bool.TryParse(Args.ElementAt(Args.ToList().IndexOf("--test") + 1), out bool parseTest)) IsTest = parseTest;
            }

        }

    }
}
