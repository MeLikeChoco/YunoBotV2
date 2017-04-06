using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YunoBotV2.Services;

//I'm too lazy to change the name of the class
//so... I have to use System.Diagnostics.Stopwatch

namespace YunoBotV2.Commands
{
    [Group("stopwatch")]
    public class Stopwatches : CustomModuleBase
    {

        [Command]
        public async Task StopwatchCommand()
            => await ReplyAsync("Usage: stopwatch start/stop/time");

        [Command("start")]
        [Summary("Start a stopwatch")]
        public async Task StartCommand()
        {

            if (Cache.Stopwatches.ContainsKey(Context.Guild.Id))
            {
                await ReplyAsync(":stopwatch: There is already a stopwatch running in this guild!");
                return;
            }

            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            Cache.Stopwatches.TryAdd(Context.Guild.Id, sw);

            await ReplyAsync(":stopwatch: A stopwatch has started running!");

        }

        [Command("stop")]
        [Summary("Stop the current stopwatch")]
        public async Task StopCommand()
        {

            if (!Cache.Stopwatches.TryRemove(Context.Guild.Id, out System.Diagnostics.Stopwatch sw))
            {
                await ReplyAsync(":stopwatch: There is no active stopwatch in this guild!");
                return;
            }

            TimeSpan elapsed = sw.Elapsed;
            sw.Stop(); 

            await ReplyAsync($":stopwatch: You have stopped the watch at {elapsed.Days} day(s), {elapsed.Hours} hour(s), {elapsed.Minutes} minute(s), and {elapsed.Seconds} second(s).");

        }

        [Command("time")]
        [Summary("Gets the current time on stopwatch")]
        public async Task TimeCommand()
        {

            if(!Cache.Stopwatches.TryGetValue(Context.Guild.Id, out System.Diagnostics.Stopwatch sw))
            {
                await ReplyAsync(":stopwatch: No stopwatches currently running in this guild!");
                return;
            }

            TimeSpan elapsed = sw.Elapsed;
            await ReplyAsync($":stopwatch: {elapsed.Days} day(s), {elapsed.Hours} hour(s), {elapsed.Minutes} minute(s), and {elapsed.Seconds} second(s) has elapsed.");

        }

    }
}
