using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
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

        private DiscordSocketClient _client;
        private CommandService _cmdService;
        private Random _random;
        private IServiceProvider _services;

        public Events(DiscordSocketClient client,
            CommandService cmdService)
        {

            _client = client;
            _cmdService = cmdService;

        }

        public async Task ReadyHerWeapons()
        {

            BuildGuildSettings();
            BuildServices();
            await BuildCommandHandler();

            _client.Ready -= ReadyHerWeapons;

        }

        private void BuildGuildSettings()
        {

            new GuildSettingContext(_client.Guilds);

        }

        private void BuildServices()
        {

            _random = new Random();

            _services = new ServiceCollection()
                .AddDbContext<GuildSettingContext>()
                .AddSingleton(_random)
                .AddSingleton(new InteractiveService(_client, TimeSpan.FromSeconds(60)))
                .AddSingleton(new Zalgo(_random))
                .AddSingleton<Web>()
                .AddSingleton<BotSettings>()
                .AddSingleton<Tokens>()
                .BuildServiceProvider();

        }

        private async Task BuildCommandHandler()
        {

            _client.MessageReceived += CommandHandler;
            await _cmdService.AddModulesAsync(Assembly.GetEntryAssembly());

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
                || possibleCmd.HasMentionPrefix(_client.CurrentUser, ref argPos)))
            {

                var context = new SocketCommandContext(_client, possibleCmd);

                if (message.Channel is SocketDMChannel)
                    Logger.Log("Info", "Command", $"{possibleCmd.Author.Username} in DM's");
                else
                    Logger.Log("Info", "Command", $"{possibleCmd.Author.Username} from {(possibleCmd.Channel as SocketTextChannel).Guild.Name}");

                Logger.Log("Info", "Command", possibleCmd.Content);

                try
                {

                    var result = await _cmdService.ExecuteAsync(context, argPos, _services);

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

        public async Task GiveAutoRoles(SocketGuildUser user)
        {

            using (var db = new GuildSettingContext())
            {

                var setting = await db.FindAsync<Guild>(user.Guild.Id);
                var roles = user.Guild.Roles.Where(role => setting.AutoRoles.Contains(role.Id));

                await user.AddRolesAsync(roles);

            }

        }

        public async Task GenGuildSetting(SocketGuild guild)
        {

            using (var db = new GuildSettingContext())
            {

                await db.AddAsync(new Guild(guild.Id));
                await db.SaveChangesAsync();

            }

        }

        public Task Log(LogMessage message)
        {

            Logger.Log(message.Severity.ToString(), message.Source, message.Message, message.Exception);

            return Task.CompletedTask;

        }

    }
}
