using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Discord;
using System.Collections.Concurrent;
using YunoBotV2.Commands.Attributes;
using YunoBotV2.Services;

namespace YunoBotV2.Commands
{
    public class Info : CustomModuleBase
    {

        private CommandService _commandService;
        private Zalgo _zalgoService;

        public Info(CommandService commandServiceParams, Zalgo zalgoServiceParams)
        {
            _commandService = commandServiceParams;
            _zalgoService = zalgoServiceParams;

        }

        [Command("channel")]
        [Summary("Get a tag cloud for a channel")]
        [Cooldown(10)]
        [RequireContext(ContextType.Guild)]
        public async Task ChannelCommand(SocketTextChannel channel)
        {

            IEnumerable<IMessage> messages = await channel.GetMessagesAsync(1000).Flatten();
            IEnumerable<string> content = messages.Select(message => message.Content);
            var count = new ConcurrentDictionary<string, int>();

            Parallel.ForEach(content, str =>
            {
                var array = str.Split(' ');

                //im sure no message is long enough to warrant a parallel foreach
                foreach (var token in array)
                {
                    count.AddOrUpdate(token, 1, (key, oldvalue) => ++oldvalue);
                }
            });

            IEnumerable<KeyValuePair<string, int>> top10 = count.Where(kv => kv.Key.Length > 3 && kv.Key != string.Empty).OrderByDescending(kv => kv.Value).Take(10);

            var organizedResults = new StringBuilder($"```Here are the top 10 words used in the channel in accord to the last {messages.Count()} messages\n\n");

            foreach(var kv in top10)
            {
                organizedResults.AppendLine($"{kv.Key.Replace("`", "").Replace("\n", "")} >> {kv.Value} times");
            }

            organizedResults.Append("```");
            await ReplyAsync(organizedResults.ToString());

        }

        //i opted to hand craft my help command because I feel it's easier to control
        [Command("help")]
        [Summary("Get a help ")]
        [Cooldown(30)]
        public async Task HelpCommand(int page = 0)
        {

            StringBuilder organizedResults;
            var prefix = "e$";

            switch (page)
            {

                case 1:
                    organizedResults = new StringBuilder("```AQWorlds Help Menu\n");
                    organizedResults.AppendLine($"{"aqw search <search terms>".PadRight(30)}| Search the AQWorlds wiki for information!");
                    organizedResults.AppendLine($"{"aqw user <the user's name>".PadRight(30)}| Dump a user's information into chat!");
                    break;
                case 2:
                    organizedResults = new StringBuilder("```Nsfw/Sfw Help Menu\n");
                    organizedResults.AppendLine($"{"nsfw <optional: search tags>".PadRight(30)}| Get a nsfw picture from a random booru!");
                    organizedResults.AppendLine($"{"sfw <optional: search tags>".PadRight(30)}| Get a sfw picture from a random booru!");
                    break;
                case 3:
                    organizedResults = new StringBuilder("```Colors Help Menu\n");
                    organizedResults.AppendLine($"{"color".PadRight(30)}| Get a random color!");
                    organizedResults.AppendLine($"{"color search <search terms>".PadRight(30)}| Get a color related to your search!");
                    organizedResults.AppendLine($"{"color <hexcode>".PadRight(30)}| Get the color associated with the hexcode!");
                    break;
                case 4:
                    organizedResults = new StringBuilder("```Food Help Menu\n");
                    organizedResults.AppendLine($"{"recipe <search terms>".PadRight(30)}| Get a random recipe based on your search!");
                    break;
                case 5:
                    organizedResults = new StringBuilder("```Fun Help Menu\n");
                    organizedResults.AppendLine($"{"randomuser".PadRight(30)}| Generate a fake random person!");
                    organizedResults.AppendLine($"{"translate <from> <to> <text>".PadRight(30)}| Translate some text!");
                    organizedResults.AppendLine($"{"yesorno <question>".PadRight(30)}| Get a yes or no answer on the question!");
                    organizedResults.AppendLine($"{"wallpaper <optional: search terms>".PadRight(30)}| Get a random wallpaper related to your search!");
                    organizedResults.AppendLine($"{"garfield".PadRight(30)}| Get a random garfield comic!");
                    organizedResults.AppendLine($"{"zalgo <text>".PadRight(30)}| {_zalgoService.GetZalgo("Totally nice text!")}");
                    if(Context.Channel is SocketTextChannel && (Context.User as SocketGuildUser).GuildPermissions.Administrator)
                    {
                        organizedResults.AppendLine($"{"zalgod <text>".PadRight(30)}| Same as above, but deletes user message!");
                    }
                    organizedResults.AppendLine($"{"playing".PadRight(30)}| Get a list of what games people are playing!");
                    organizedResults.AppendLine($"{"gif <optional: search terms>".PadRight(30)}| Get a gif related to your search!");
                    organizedResults.AppendLine($"{"norris <optional: name>".PadRight(30)}| Get a Chuck Norris joke, with your name if you want!");
                    organizedResults.AppendLine($"{"for the glory of satan".PadRight(30)}| WHY? FOR THE GLORY OF SATAN OF COURSE!");
                    organizedResults.AppendLine($"{"roll <optional: sides of dice>".PadRight(30)}| Roll a dice with a custom amount of sides or just 6!");
                    organizedResults.AppendLine($"{"coinflip".PadRight(30)}| Flip a coin!");
                    organizedResults.AppendLine($"{"ascii <text>".PadRight(30)}| Turn your text into ascii art!");
                    organizedResults.AppendLine($"{"rr <bullets> <max rounds>".PadRight(30)}| Play some russian roulette!");
                    organizedResults.AppendLine($"{"fuck you <user mention/id>".PadRight(30)}| Ultimate kek.");
                    break;
                case 6:
                    organizedResults = new StringBuilder("```League of Legends Help Menu\n");
                    organizedResults.AppendLine($"{"lol <champion name>"}| Return stats on a champion!");
                    break;
                case 7:
                    organizedResults = new StringBuilder("```Osu Help Menu\n");
                    organizedResults.AppendLine($"{"osur <mode: 1/2/3>".PadRight(30)}| Get a random osu beatmap!");
                    organizedResults.AppendLine($"{"osuu <username>".PadRight(30)}| Fetch info on an user!");
                    break;
                case 8:
                    organizedResults = new StringBuilder("```Steam Help Menu\n");
                    organizedResults.AppendLine($"{"steam deals".PadRight(30)}| Returns the top 10 steam specials from frontpage!");
                    organizedResults.AppendLine($"{"steam top".PadRight(30)}| Returns the top 10 steam sellers from frontpage!");
                    organizedResults.AppendLine($"{"steam new".PadRight(30)}| Retrusn the top 10 popular new releases from frontpage!");
                    break;
                case 9:
                    organizedResults = new StringBuilder("```Weaboo Help Menu\n");
                    organizedResults.AppendLine($"{"anime <search terms>".PadRight(30)}| Search for anime!");
                    organizedResults.AppendLine($"{"manga <search terms>".PadRight(30)}| Search for manga!");
                    organizedResults.AppendLine($"{"character <search terms>".PadRight(30)}| Search for anime/manga characters!");
                    break;
                case 10:
                    organizedResults = new StringBuilder("```Wiki Help Menu\n");
                    organizedResults.AppendLine($"{"wiki <search terms>".PadRight(30)}| Search the Wikipedia!");
                    organizedResults.AppendLine($"{"wikia <community> <search terms>".PadRight(30)}| Search the Wikia!");
                    organizedResults.AppendLine($"{"gamepedia <community> <search terms>".PadRight(30)}| Search Gamepedia!");
                    break;
                default:
                    organizedResults = new StringBuilder("```The Defacto Help Menu\n");
                    organizedResults.AppendLine("1. AQWorlds");
                    organizedResults.AppendLine("2. NSFW/SFW");
                    organizedResults.AppendLine("3. Colors");
                    organizedResults.AppendLine("4. Food");
                    organizedResults.AppendLine("5. Fun");
                    organizedResults.AppendLine("6. League of Legends");
                    organizedResults.AppendLine("7. Osu");
                    organizedResults.AppendLine("8. Steam");
                    organizedResults.AppendLine("9. Weaboo");
                    organizedResults.AppendLine("10. Wiki");
                    organizedResults.AppendLine($"\nUse {prefix}help <number> for its menu!");
                    break;

            }

            organizedResults.Append($"Prefix: {prefix}```");

            await ReplyAsync(organizedResults.ToString());

        }

    }
}
