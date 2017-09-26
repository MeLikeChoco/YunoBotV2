using Discord;
using Discord.Commands;
using Discord.Rest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunoV3.Modules
{
    public class CustomBase : ModuleBase<SocketCommandContext>
    {

        /// <summary>
        /// $"There were no {objects} found with `{input}`!"
        /// </summary>
        /// <param name="objects">The expected results (plural form)</param>
        /// <param name="input">The entered search term</param>
        /// <returns>Task</returns>
        public Task NoResultError(string objects, string input)
            => ReplyAsync($"There were no {objects} found with `{input}`!");

        /// <summary>
        /// $"There was nothing found with `{input}`!"
        /// </summary>
        /// <param name="input">The entered search term</param>
        /// <returns>Task</returns>
        public Task NoResultError(string input)
            => ReplyAsync($"There was nothing found with `{input}`!");

        /// <summary>
        /// Upload file to channel
        /// </summary>
        /// <param name="stream">The stream</param>
        /// <param name="filename">The filename</param>
        /// <returns>Task</returns>
        public Task<RestUserMessage> UploadAsync(Stream stream, string filename)
            => Context.Channel.SendFileAsync(stream, filename);

        /// <summary>
        /// Send an embed message
        /// </summary>
        /// <param name="embed">The embed to send</param>
        /// <returns>Task</returns>
        public Task<RestUserMessage> SendEmbedAsync(Embed embed)
            => Context.Channel.SendMessageAsync("", embed: embed);

    }
}
