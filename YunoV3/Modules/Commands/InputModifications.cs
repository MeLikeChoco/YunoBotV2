using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YunoV3.Objects;
using YunoV3.Services;

namespace YunoV3.Modules.Commands
{
    public class InputModifications : CustomBase
    {

        private Zalgo _zalgo;

        public InputModifications(Zalgo zalgo)
            => _zalgo = zalgo;

        [Command("zalgo")]
        [Summary("Y̴͎̓O̲̓͡U̧͙̍ ̸̯͂W̻͋͢Ï̴͕L͎̾̕L͙ͮ́ ̫ͫ͡O̸͔̎B̴̤̓E̡̹ͩY̪ͨ͘҉̙ͮ͝")]
        public Task SendZalgo([Remainder]string input)
            => ReplyAsync(_zalgo.GetZalgo(input));

        [Command("zalgod")]
        [Summary("Y̴͎̓O̲̓͡U̧͙̍ ̸̯͂W̻͋͢Ï̴͕L͎̾̕L͙ͮ́ ̫ͫ͡O̸͔̎B̴̤̓E̡̹ͩY̪ͨ͘҉̙ͮ͝")]
        [RequireBotPermission(GuildPermission.ManageMessages)]
        public Task SendZalgod([Remainder]string input)
        {

            Context.Message.DeleteAsync();

            return ReplyAsync(_zalgo.GetZalgo(input));

        }

        [Command("vaporwave")]
        [Summary("ｖａｐｏｒｗａｖｅ")]
        public Task Vaporwave([Remainder]string text = "aesthetic")
        {

            var builder = new StringBuilder();

            foreach (var c in text)
            {

                if (c >= '!' && c <= '~')
                    builder.Append((char)(c + 65248));
                else if (c == ' ')
                    builder.Append("\u3000");
                else
                    builder.Append(c);

            }

            return ReplyAsync(builder.ToString());

        }

    }
}
