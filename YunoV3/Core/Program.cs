using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YunoV3.Objects;

namespace YunoV3.Core
{
    public class Program
    {

        static async Task Main(string[] args)
        {

            Logger.Initialize();
            Logger.Log("YunoBot is up and running! Ready to kill for love and attention!");

            var events = new Events();
            await events.GoOnAKillingSpree();

            await Task.Delay(-1);

        }

    }
}
