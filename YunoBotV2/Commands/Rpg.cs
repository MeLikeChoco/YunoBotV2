using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using YunoBotV2.Services;
using YunoBotV2.Services.Extensions;
using YunoBotV2.Services.Rpg;
using YunoBotV2.Services.Rpg.NameGen;
using YunoBotV2.Services.WebServices;

namespace YunoBotV2.Commands
{

    [Group("rpg")]
    public class Rpg : CustomModuleBase
    {
        
        private Web _web;

        public Rpg(Web web)
        {

            _web = web;

        }

        [Group("namegen")]
        public class NameGenerator : CustomModuleBase
        {            

            [Command]
            public async Task GenerateNamesCommand(string name, int amount, int type = 0)
            {

                IEnumerable<string> results;

                try
                {

                    results = Generator.GenerateNames(name, amount, type);

                }
                catch (ArgumentOutOfRangeException a)
                {

                    await ReplyAsync(a.Message);
                    return;

                }
                catch(TargetInvocationException t)
                {

                    await ReplyAsync(t.InnerException.Message);
                    return;

                }

                if(results == null)
                {

                    await ReplyAsync($"There was no generator with the name of `{name}`!");
                    return;

                }

                var builder = new StringBuilder("```");

                builder.AppendLine();

                foreach(var result in results)
                {

                    builder.AppendLine(result);

                }

                builder.Append("```");
                await ReplyAsync(builder.ToString());

            }

        }

    }
}
