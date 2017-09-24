using Discord;
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

namespace YunoV3.Core
{
    public class Events
    {

        private DiscordSocketClient _client;
        private CommandService _cmdService;
        private BotSettings _botSettings;
        private IServiceProvider _services;

        public Events(DiscordSocketClient client, 
            CommandService cmdService)
        {

            _client = client;
            _cmdService = cmdService;
            _botSettings = new BotSettings();

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

            _services = new ServiceCollection()
                .AddSingleton<Random>()
                .AddDbContext<GuildSettingContext>()
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

            if(!(message.Channel is SocketDMChannel))
            {

                using (var db = new GuildSettingContext())
                {

                    var id = (message.Channel as SocketTextChannel).Guild.Id;
                    prefix = (await db.FindAsync<Guild>(id)).Prefix;

                }

            }

            var possibleCmd = message as SocketUserMessage;
            var argPos = 0;

            if(possibleCmd.Content.Trim() != prefix
                && (possibleCmd.HasStringPrefix(prefix, ref argPos, StringComparison.InvariantCultureIgnoreCase)
                || possibleCmd.HasMentionPrefix(_client.CurrentUser, ref argPos)))
            {

                var context = new SocketCommandContext(_client, possibleCmd);

                if (message.Channel is SocketDMChannel)
                    Logger.Log("Info", "Command", $"{possibleCmd.Author.Username} in DM's");
                else
                    Logger.Log("Info", "Command", $"{possibleCmd.Author.Username} from {(possibleCmd.Channel as SocketTextChannel).Guild.Name}");

                Logger.Log("Info", "Command", possibleCmd.Content);

                var result = await _cmdService.ExecuteAsync(context, argPos, _services);

                if (!result.IsSuccess)
                {

                    if (result.ErrorReason.ToLower().Contains("unknown command"))
                        return;
                    else if (result.ErrorReason.ToLower().Contains("you are currently in timeout"))
                        await context.Channel.SendMessageAsync("Please wait 5 seconds between each type of paginator command!");
                    else
                        await context.Channel.SendMessageAsync("There was an error in the command.");

                    //await context.Channel.SendMessageAsync("https://goo.gl/JieFJM");

                    Logger.Log("Error", "Error", result.ErrorReason);
                    //debug purposes
                    //await context.Channel.SendMessageAsync($"**Error:** {result.ErrorReason}");

                }

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
