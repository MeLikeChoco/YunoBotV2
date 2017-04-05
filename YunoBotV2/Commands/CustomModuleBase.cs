using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace YunoBotV2.Commands
{
    public class CustomModuleBase : ModuleBase
    {

        /// <summary>
        /// "There was an error using this command! Please check input or try again later."
        /// </summary>
        /// <returns></returns>
        public async Task DefaultErrorMessage()
            => await Context.Channel.SendMessageAsync("There was an error using this command! Please check input or try again later.");

    }
}
