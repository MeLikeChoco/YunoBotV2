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

namespace YunoBotV2.Commands
{
    public class CustomModuleBase : InteractiveModuleBase<SocketCommandContext>
    {

        /// <summary>
        /// "There was an error using this command! Please check input or try again later."
        /// </summary>
        /// <returns></returns>
        public async Task DefaultErrorMessage()
            => await Context.Channel.SendMessageAsync("There was an error using this command! Please check input or try again later.");

        /// <summary>
        /// "You need to at least be able to search for something..."
        /// </summary>
        /// <returns></returns>
        public async Task SearchErrorMessage()
            => await Context.Channel.SendMessageAsync("You need to at least be able to search for something...");

    }
}
