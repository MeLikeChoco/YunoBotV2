using Discord.Commands;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace YunoBotV2.Commands.Attributes
{
    public class Cooldown : PreconditionAttribute
    {

        private static ConcurrentDictionary<string, ConcurrentDictionary<ulong, Stopwatch>> _cooldowns = new ConcurrentDictionary<string, ConcurrentDictionary<ulong, Stopwatch>>();
        private static Timer _cleanCooldowns = new Timer(CleanCooldowns, null, 100, 100);
        private int _cooldown;

        public Cooldown(int seconds)
        {
            _cooldown = seconds;
        }

        public override Task<PreconditionResult> CheckPermissions(ICommandContext context, CommandInfo command, IDependencyMap map)
        {

            if (_cooldowns.TryAdd(command.Name, new ConcurrentDictionary<ulong, Stopwatch>()))
            {

                var stopwatch = new Stopwatch();
                stopwatch.Start();

                if (_cooldowns[command.Name].TryAdd(context.Guild.Id, stopwatch))
                {
                    return Task.FromResult(PreconditionResult.FromSuccess());
                }

            }
            else
            {

                if (_cooldowns[command.Name].TryGetValue(context.Guild.Id, out var stopwatch))
                {

                    if (stopwatch.Elapsed > TimeSpan.FromSeconds(_cooldown))
                    {
                        _cooldowns[command.Name].TryRemove(context.Guild.Id, out stopwatch);
                        return Task.FromResult(PreconditionResult.FromSuccess());
                    }
                    else
                    {
                        var secondsLeft = TimeSpan.FromSeconds(_cooldown) - stopwatch.Elapsed;
                        context.Channel.SendMessageAsync($"The command is on cooldown! Remaining time left: {secondsLeft.Seconds} second.");
                        return Task.FromResult(PreconditionResult.FromError($"The command is on cooldown! Remaining time left: {secondsLeft.Seconds} second."));
                    }

                }
                else
                {
                    stopwatch = new Stopwatch();
                    stopwatch.Start();
                    _cooldowns[command.Name].TryAdd(context.Guild.Id, stopwatch);
                    return Task.FromResult(PreconditionResult.FromSuccess());
                }

            }

            return Task.FromResult(PreconditionResult.FromSuccess());

        }

        private static void CleanCooldowns(object state)
        {

            foreach (var command in _cooldowns)
            {

                foreach (var kv in command.Value)
                {

                    if (kv.Value.Elapsed > TimeSpan.FromSeconds(60))
                    {
                        _cooldowns[command.Key].TryRemove(kv.Key, out var blah);
                    }

                }

            }

        }

    }
}
