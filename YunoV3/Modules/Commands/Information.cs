using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YunoV3.Extensions;
using YunoV3.Objects.Database.Guilds;

namespace YunoV3.Modules.Commands
{
    public class Information : CustomInteractiveBase
    {

        private CommandService _cmdService;
        private InteractiveService _interactive;
        private GuildSettingContext _dbContext;
        private Random _random;
        private Guild _guild;

        private static readonly PaginatedAppearanceOptions _paginatedOptions = new PaginatedAppearanceOptions()
        {

            DisplayInformationIcon = false,
            JumpDisplayOptions = JumpDisplayOptions.Never,
            FooterFormat = "Message will be deleted in 60 seconds! | Page {0}/{1}"

        };

        public Information(CommandService commandService,
            InteractiveService interactiveService,
            GuildSettingContext dbContext,
            Random random)
        {

            _cmdService = commandService;
            _interactive = interactiveService;
            _dbContext = dbContext;
            _random = random;

        }

        protected override void BeforeExecute(CommandInfo command)
        {

            if(Context.Channel is SocketTextChannel)
                _guild = _dbContext.Find<Guild>(Context.Guild.Id);

        }

        [Command("uptime")]
        [Summary("Get the bot's uptime")]
        public Task GetUptime()
        {

            var uptime = DateTime.Now.Subtract(Process.GetCurrentProcess().StartTime);

            var str = "The bot has been up for ";

            if (uptime.Days != 0)
            {

                str += $"**{uptime.Days}** ";

                if (uptime.Days > 1)
                    str += "days, ";
                else
                    str += "day, ";

            }

            if (uptime.Hours != 0)
            {

                str += $"**{uptime.Hours}** ";

                if (uptime.Hours > 1)
                    str += "hours, ";
                else
                    str += "hour, ";

            }

            if (uptime.Minutes != 0)
            {

                str += $"**{uptime.Minutes}** ";

                if (uptime.Minutes > 1)
                    str += "minutes, ";
                else
                    str += "minute, ";

            }

            if (uptime.Seconds != 0)
            {

                str += $"**{uptime.Seconds}** ";

                if (uptime.Seconds > 1)
                    str += "seconds.";
                else
                    str += "second.";

            }

            return ReplyAsync(str);

        }

        [Command("help")]
        [Summary("Defacto help command")]
        public Task GetCommands()
        {

            var groups = _cmdService.Commands
                .Where(command => command.CheckPreconditionsAsync(Context).Result.IsSuccess)
                .Batch(7);
            var pages = new List<string>(3);

            foreach(var commands in groups)
                pages.Add(GenHelpPage(commands));

            var author = new EmbedAuthorBuilder()
                .WithName("Defacto Help Menu")
                .WithIconUrl("http://68.media.tumblr.com/fd9518b1814be8491b840b232bbc819e/tumblr_inline_no5d65HfpV1qk8x8b_540.png");

            var pager = new PaginatedMessage()
            {

                Author = author,
                Color = new Color(255, 183, 197),
                Options = _paginatedOptions,
                Pages = pages

            };

            return PagedReplyAsync(pager);

        }

        [Command("help")]
        [Summary("Get help on a specific command")]
        public Task GetSomeHelp([Remainder]string input)
        {

            var commands = _cmdService.Commands
                .Where(command => command.Name == input && command.CheckPreconditionsAsync(Context).Result.IsSuccess);

            if (commands.Count() != 0)
                return ReplyAsync($"```fix\n{GenHelpPage(commands)}```");
            else
                return NoResultError("commands", input);

        }

        private string GenHelpPage(IEnumerable<CommandInfo> commands)
        {

            var prefix = "e$";

            if (_guild != null)
                prefix = _guild.Prefix;

            var str = "";

            foreach (var command in commands)
            {

                str += $"{prefix}{command.Name} ";

                foreach (var parameter in command.Parameters)
                {

                    if (parameter.IsOptional)
                        str += $"<optional: {parameter.Name}>";
                    else
                        str += $"<{parameter.Name}>";

                }

                str += $"\n{command.Summary}\n\n";

            }

            return str;

        }

    }
}
