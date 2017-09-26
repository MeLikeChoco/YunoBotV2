using Discord.Addons.Interactive;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunoV3.Modules
{
    public class CustomInteractiveBase : InteractiveBase<SocketCommandContext>
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
        public Task UploadAsync(Stream stream, string filename)
            => Context.Channel.SendFileAsync(stream, filename);

    }
}
