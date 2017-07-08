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
using YunoBotV2.Services.Extensions;

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

            var messages = await channel.GetMessagesAsync(1000).Flatten();
            var content = messages.Select(message => message.Content);
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

            var top10 = count.Where(kv => kv.Key.Length > 3 && kv.Key != string.Empty).OrderByDescending(kv => kv.Value).Take(10);

            var organizedResults = new StringBuilder($"```Here are the top 10 words used in the channel in accord to the last {messages.Count()} messages\n\n");

            foreach(var kv in top10)
            {
                organizedResults.AppendLine($"{kv.Key.Replace("`", "").Replace("\n", "")} >> {kv.Value} times");
            }

            organizedResults.Append("```");
            await ReplyAsync(organizedResults.ToString());

        }

        [Command("prefix")]
        [Summary("Set custom prefix")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [Priority(0)]
        public async Task PrefixCommand([Remainder]string prefix)
        {

            await Database.SavePrefix(Context.Guild.Id, prefix);
            await ReplyAsync($"Prefix has been set to: {prefix}");
            
        }

        [Command("prefix")]
        [Summary("Set custom prefix")]
        [RequireOwner]
        [Priority(1)]
        public async Task ForceSetPrefixCommand([Remainder]string prefix)
        {

            await Database.SavePrefix(Context.Guild.Id, prefix);
            await ReplyAsync($"Prefix has been set to: {prefix}");

        }

        //i opted to hand craft my help command because I feel it's easier to control
        [Command("help")]
        [Summary("Get a help ")]
        public async Task HelpCommand(int page = 0)
        {

            StringBuilder organizedResults;

            if (Database.GetPrefix(Context.Guild.Id, out string prefix)) { }
            else
                prefix = "e$";

            switch (page)
            {

                case 1:
                    organizedResults = new StringBuilder("```AQWorlds Help Menu\n\n");
                    organizedResults.AppendLineHelp($"aqw search <search terms>| Search the AQWorlds wiki for information!");
                    organizedResults.AppendLineHelp($"aqw user <the username>| Dump a user's information into chat!");
                    break;
                case 2:
                    organizedResults = new StringBuilder("```Nsfw/Sfw Help Menu\n\n");
                    organizedResults.AppendLineHelp($"nsfw <optional: search tags>| Get a nsfw picture from a random booru!");
                    organizedResults.AppendLineHelp($"sfw <optional: search tags>| Get a sfw picture from a random booru!");
                    break;
                case 3:
                    organizedResults = new StringBuilder("```Colors Help Menu\n\n");
                    organizedResults.AppendLineHelp($"color| Get a random color!");
                    organizedResults.AppendLineHelp($"color search <search terms>| Get a color related to your search!");
                    organizedResults.AppendLineHelp($"color <hexcode>| Get the color associated with the hexcode!");
                    break;
                case 4:
                    organizedResults = new StringBuilder("```Food Help Menu\n\n");
                    organizedResults.AppendLineHelp($"recipe <search terms>| Get a random recipe based on your search!");
                    break;
                case 5:
                    organizedResults = new StringBuilder("```Fun Help Menu\n\n");
                    organizedResults.AppendLineHelp($"needsmorejpeg| Applies MOAR JPEG to a previous picture sent in chat!");
                    organizedResults.AppendLineHelp($"battle <mention/id>| Battle a user!");
                    organizedResults.AppendLineHelp($"battler| Battle a random user!");
                    organizedResults.AppendLineHelp($"randomuser| Generate a fake random person!");
                    organizedResults.AppendLineHelp($"translate <from> <to> <text>| Translate some text!");
                    organizedResults.AppendLineHelp($"yesorno <question>| Get a yes or no answer on the question!");
                    organizedResults.AppendLineHelp($"wallpaper <optional: search terms>| Get a random wallpaper related to your search!");
                    organizedResults.AppendLineHelp($"garfield| Get a random garfield comic!");
                    organizedResults.AppendLineHelp($"vaporwave| ｔｈｉｓ　ｂｏｔ　ｖａｐｅｓ");
                    organizedResults.AppendLineHelp($"zalgo <text>| {_zalgoService.GetZalgo("Totally nice text!")}");
                    if(Context.Channel is SocketTextChannel && (Context.User as SocketGuildUser).GuildPermissions.Administrator)
                    {
                        organizedResults.AppendLineHelp($"zalgod <text>| Same as above, but deletes user message!");
                    }
                    organizedResults.AppendLineHelp($"playing| Get a list of what games people are playing!");
                    organizedResults.AppendLineHelp($"gif <optional: search terms>| Get a gif related to your search!");
                    organizedResults.AppendLineHelp($"norris <optional: name>| Get a Chuck Norris joke, with your name if you want!");
                    organizedResults.AppendLineHelp($"for the glory of satan| WHY? FOR THE GLORY OF SATAN OF COURSE!");
                    organizedResults.AppendLineHelp($"roll <optional: sides of dice>| Roll a dice with a custom amount of sides or just 6!");
                    organizedResults.AppendLineHelp($"coinflip| Flip a coin!");
                    organizedResults.AppendLineHelp($"ascii <text>| Turn your text into ascii art!");
                    organizedResults.AppendLineHelp($"rr <bullets> <max rounds>| Play some russian roulette!");
                    organizedResults.AppendLineHelp($"fuck you <user mention/id>| Ultimate kek.");
                    organizedResults.AppendLineHelp($"imdb <search>| Search imdb's database!");
                    break;
                case 6:
                    organizedResults = new StringBuilder("```League of Legends Help Menu\n\n");
                    organizedResults.AppendLineHelp($"lol <champion name>| Return stats on a champion!");
                    break;
                case 7:
                    organizedResults = new StringBuilder("```Minecraft Help Menu\n\n");
                    organizedResults.AppendLineHelp($"mc avatar <name>| Get a user's avatar!");
                    organizedResults.AppendLineHelp($"mc skin <name>| Get a user's skin!");
                    organizedResults.AppendLineHelp($"mc rawskin <name>| Get a user's skin file!");
                    organizedResults.AppendLineHelp($"mc render head <name>| Render a user's head!");
                    organizedResults.AppendLineHelp($"mc render body <name>| Render a user's body!");
                    break;
                case 8:
                    organizedResults = new StringBuilder("```Osu Help Menu\n\n");
                    organizedResults.AppendLineHelp($"osur <mode: 1/2/3>| Get a random osu beatmap!");
                    organizedResults.AppendLineHelp($"osuu <username>| Fetch info on an user!");
                    break;
                case 9:
                    organizedResults = new StringBuilder("```Steam Help Menu\n\n");
                    organizedResults.AppendLineHelp($"steam deals| Returns the top 10 steam specials from frontpage!");
                    organizedResults.AppendLineHelp($"steam top| Returns the top 10 steam sellers from frontpage!");
                    organizedResults.AppendLineHelp($"steam new| Returns the top 10 popular new releases from frontpage!");
                    break;
                case 10:
                    organizedResults = new StringBuilder("```Weaboo Help Menu\n\n");
                    organizedResults.AppendLineHelp($"anime <search terms>| Search for anime!");
                    organizedResults.AppendLineHelp($"manga <search terms>| Search for manga!");
                    organizedResults.AppendLineHelp($"character <search terms>| Search for anime/manga characters!");
                    break;
                case 11:
                    organizedResults = new StringBuilder("```Wiki Help Menu\n\n");
                    organizedResults.AppendLineHelp($"wiki <search terms>| Search the Wikipedia!");
                    organizedResults.AppendLineHelp($"wikia <community> <search terms>| Search the Wikia!");
                    organizedResults.AppendLineHelp($"gamepedia <community> <search terms>| Search Gamepedia!");
                    break;
                case 12:
                    organizedResults = new StringBuilder("```Dump Help Menu\n\n");
                    organizedResults.AppendLineHelp($"dump roles| Dump a list of roles");
                    organizedResults.AppendLineHelp($"dump users| Dump a list of users");
                    organizedResults.AppendLineHelp($"dump bans| Dump a list of bans");
                    organizedResults.AppendLineHelp($"dump text channels| Dump a list of text channels");
                    organizedResults.AppendLineHelp($"dump voice channels| Dump a list of voice channels");
                    organizedResults.AppendLineHelp($"dump guild| Dump information on the guild");
                    organizedResults.AppendLineHelp($"dump user <optional: user>| Dump information on a user. default: you");
                    organizedResults.AppendLineHelp($"dump user <role name>| Dump information on role");
                    break;
                default:
                    organizedResults = new StringBuilder("```The Defacto Help Menu\n\n");
                    organizedResults.AppendLine("1. AQWorlds");
                    organizedResults.AppendLine("2. NSFW/SFW");
                    organizedResults.AppendLine("3. Colors");
                    organizedResults.AppendLine("4. Food");
                    organizedResults.AppendLine("5. Fun");
                    organizedResults.AppendLine("6. League of Legends");
                    organizedResults.AppendLine("7. Minecraft");
                    organizedResults.AppendLine("8. Osu");
                    organizedResults.AppendLine("9. Steam");
                    organizedResults.AppendLine("10. Weaboo");
                    organizedResults.AppendLine("11. Wiki");
                    organizedResults.AppendLine("12. Dump");
                    organizedResults.AppendLine($"\nUse {prefix}help <number> for its menu!");
                    break;

            }

            organizedResults.Append($"Prefix: {prefix}```");

            await ReplyAsync(organizedResults.ToString());

        }

    }
}
