using Discord;
using Discord.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunoV3.Extensions
{
    public static class RestUserMessageExtensions
    {

        public static Task EditAsync(this RestUserMessage message, Embed embed)
            => message.EditAsync("", embed);

        public static Task EditAsync(this RestUserMessage message, string replacement, Embed embed = null)
            => message.ModifyAsync(properties =>
            {

                properties.Content = replacement;
                properties.Embed = embed;

            });
    }
}
