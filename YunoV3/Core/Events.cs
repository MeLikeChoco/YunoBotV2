using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.Net.Providers.WS4Net;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using YunoV3.Objects;
using YunoV3.Objects.Database.Guilds;
using YunoV3.Objects.Exceptions;
using YunoV3.Services;

namespace YunoV3.Core
{
    public class Events
    {

        private DiscordSocketClient _discordSocketclient;
        private CommandService _commandService;
        private Random _random;
        private IServiceProvider _services;

        private DiscordSocketConfig _discordSocketConfig
        {

            get
            {

                var config = new DiscordSocketConfig
                {

                    LogLevel = LogSeverity.Verbose,
                    MessageCacheSize = 1000

                };

                if (Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor == 1)
                    config.WebSocketProvider = WS4NetProvider.Instance;

                return config;

            }

        }

        private CommandServiceConfig _commandServiceConfig => new CommandServiceConfig
        {

            LogLevel = LogSeverity.Verbose,
            DefaultRunMode = RunMode.Async //the overhead is nothing to be worried about

        };

        public Events()
        {

            _discordSocketclient = new DiscordSocketClient(_discordSocketConfig);
            _commandService = new CommandService(_commandServiceConfig);

            _discordSocketclient.Log += Log;
            _commandService.Log += Log;
            _discordSocketclient.Ready += ReadyHerWeapons;
            _discordSocketclient.JoinedGuild += GenGuildSetting;
            _discordSocketclient.UserJoined += GiveAutoRoles;

        }

        public async Task GoOnAKillingSpree()
        {

            var token = new BotSettings().IsTest ? new Tokens().DiscordTest : new Tokens().DiscordLegit;

            await _discordSocketclient.LoginAsync(TokenType.Bot, token);
            await _discordSocketclient.StartAsync();

        }

        private async Task ReadyHerWeapons()
        {

            BuildGuildSettings();
            BuildServices();
            await BuildCommandHandler();

            _discordSocketclient.Ready -= ReadyHerWeapons;

        }

        private void BuildGuildSettings()
            => new GuildSettingContext(_discordSocketclient.Guilds);

        private void BuildServices()
        {

            _random = new Random();

            _services = new ServiceCollection()
                .AddDbContext<GuildSettingContext>()
                .AddSingleton(_random)
                .AddSingleton(new InteractiveService(_discordSocketclient, TimeSpan.FromDays(1)))
                .AddSingleton(new Zalgo(_random))
                .AddSingleton<Web>()
                .AddSingleton<BotSettings>()
                .AddSingleton<Tokens>()
                .BuildServiceProvider();

        }

        private async Task BuildCommandHandler()
        {

            _discordSocketclient.MessageReceived += CommandHandler;
            await _commandService.AddModulesAsync(Assembly.GetEntryAssembly(), _services);

        }

        private async Task CommandHandler(SocketMessage message)
        {

            if (!(message is SocketUserMessage)
                || message.Author.IsBot
                || string.IsNullOrEmpty(message.Content))
                return;

            var prefix = "e$";

            if (!(message.Channel is SocketDMChannel))
            {

                using (var db = new GuildSettingContext())
                {

                    var id = (message.Channel as SocketTextChannel).Guild.Id;
                    prefix = (await db.FindAsync<Guild>(id)).Prefix;

                }

            }

            var possibleCmd = message as SocketUserMessage;
            var argPos = 0;

            if (possibleCmd.Content.Trim() != prefix
                && (possibleCmd.HasStringPrefix(prefix, ref argPos, StringComparison.InvariantCultureIgnoreCase)
                || possibleCmd.HasMentionPrefix(_discordSocketclient.CurrentUser, ref argPos)))
            {

                var context = new SocketCommandContext(_discordSocketclient, possibleCmd);

                if (message.Channel is SocketDMChannel)
                    Logger.Log("Info", "Command", $"{possibleCmd.Author.Username} in DM's");
                else
                    Logger.Log("Info", "Command", $"{possibleCmd.Author.Username} from {(possibleCmd.Channel as SocketTextChannel).Guild.Name}");

                Logger.Log("Info", "Command", possibleCmd.Content);

                try
                {

                    var result = await _commandService.ExecuteAsync(context, argPos, _services);

                    if (!result.IsSuccess)
                    {
                        
                        if (result.ErrorReason.ToLower().Contains("unknown command"))
                            return;
                        else if (result.ErrorReason.ToLower().Contains("you are currently in timeout"))
                            await context.Channel.SendMessageAsync("Please wait 5 seconds between each type of paginator command!");

                        //await context.Channel.SendMessageAsync("https://goo.gl/JieFJM");

                        Logger.Log("Error", "Error", result.ErrorReason);
                        //debug purposes
                        //await context.Channel.SendMessageAsync($"**Error:** {result.ErrorReason}");

                    }

                }
                catch (WebServiceException)
                {

                    await context.Channel.SendMessageAsync("The current service the command uses is not available right now. Please try again later.");

                }

            }

        }

        private async Task GiveAutoRoles(SocketGuildUser user)
        {

            using (var db = new GuildSettingContext())
            {

                var setting = await db.FindAsync<Guild>(user.Guild.Id);
                var roles = user.Guild.Roles.Where(role => setting.AutoRoles.Contains(role.Id));

                await user.AddRolesAsync(roles);

            }

        }

        private async Task GenGuildSetting(SocketGuild guild)
        {

            using (var db = new GuildSettingContext())
            {

                await db.AddAsync(new Guild(guild.Id));
                await db.SaveChangesAsync();

            }

        }

        private Task Log(LogMessage message)
        {

            Logger.Log(message.Severity.ToString(), message.Source, message.Message, message.Exception);

            return Task.CompletedTask;

        }

    }
}
