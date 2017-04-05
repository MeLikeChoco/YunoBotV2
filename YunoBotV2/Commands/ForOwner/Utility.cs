using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunoBotV2.Commands.ForOwner
{
    [RequireOwner]
    public class Utility : CustomModuleBase
    {

        [Command("shutdown")]
        [Summary("Shuts down bot")]
        public Task ShutdownCommand()
            => Task.Run(() => Environment.Exit(0));

    }
}
