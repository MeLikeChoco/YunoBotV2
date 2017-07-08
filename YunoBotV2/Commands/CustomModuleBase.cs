using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Addons.InteractiveCommands;
using YunoBotV2.Services.WebServices;
using System.IO;

namespace YunoBotV2.Commands
{
    public class CustomModuleBase : InteractiveModuleBase<SocketCommandContext>
    {

        /// <summary>
        /// "There was an error using this command! Please check input or try again later."
        /// </summary>
        /// <returns></returns>
        public async Task DefaultErrorMessage()
            => await ReplyAsync("There was an error using this command! Please check input or try again later.");

        /// <summary>
        /// "You need to at least be able to search for something..."
        /// </summary>
        /// <returns></returns>
        public async Task SearchErrorMessage()
            => await ReplyAsync("You need to at least be able to search for something...");

        /// <summary>
        /// "Nothing was returned from the search! Please check input and try again."
        /// </summary>
        /// <returns></returns>
        public async Task NoResultsReturnedErrorMessage()
            => await ReplyAsync("Nothing was returned from the search! Please check input and try again.");

        /// <summary>
        /// "Too many results were returned! Please refine your search."
        /// </summary>
        /// <returns></returns>
        public async Task RefineSearchErrorMessage()
            => await ReplyAsync("Too many results were returned! Please refine your search.");

        /// <summary>
        /// Uploads a file
        /// </summary>
        /// <param name="stream">The stream containing the file information</param>
        /// <param name="fileExtension">The file extension</param>
        /// <returns></returns>
        public async Task UploadAsync(Stream stream, string fileExtension)
            => await Context.Channel.SendFileAsync(stream, fileExtension);

        public async Task MessageSender(string message)
            => await Context.User.SendMessageAsync(message);

    }
}
