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

namespace YunoBotV2.Commands
{
    public class Info : CustomModuleBase
    {

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

    }
}
