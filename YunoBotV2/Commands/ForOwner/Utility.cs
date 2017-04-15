using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YunoBotV2.Services.WebServices;

namespace YunoBotV2.Commands.ForOwner
{
    [RequireOwner]
    public class Utility : CustomModuleBase
    {

        private Web _service;

        public Utility(Web serviceParams)
        {
            _service = serviceParams;
        }

        [Command("shutdown")]
        [Summary("Shuts down bot")]
        public Task ShutdownCommand()
            => Task.Run(() => Environment.Exit(0));

        [Command("newname")]
        [Summary("Change name of bot")]
        public async Task NameChangeCommand([Remainder]string name)
            => await Context.Client.CurrentUser.ModifyAsync(x => x.Username = name);

        [Command("avatar")]
        [Summary("Change avatar picture")]
        public async Task AvatarChangeCommand()
            => await Context.Client.CurrentUser.ModifyAsync(x => x.Avatar = new Image("Configuration/avatar.jpg"));

    }
}
