using Discord;
using Discord.Commands;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace YunoBotV2.Commands
{
    [RequireOwner]    
    public class Eval : CustomModuleBase
    {

        private Stopwatch _timer = new Stopwatch();

        protected override void BeforeExecute()
        {
            _timer.Start();
        }

        [Command("eval"), Alias("evaluation", "evaluate")]
        [Summary("Dynamic code evaluation")]
        public async Task EvaluationCommand([Remainder]string rawCode)
        {

            ScriptOptions evalOptions = ScriptOptions.Default.AddReferences(Assembly.GetEntryAssembly()).AddImports(new string[]
            {

                "System",
                "System.Net",
                "System.Reflection",
                "System.Threading.Tasks",
                "System.Linq",
                "System.Collections.Generic",
                "Discord",
                "Discord.WebSocket"

            });

            string successCode, evaluation;
            Color successOrError;
            EmbedBuilder eBuilder;

            using (Context.Channel.EnterTypingState())
            {

                string cleanedCode = CleanCode(rawCode);

                try
                {

                    Object Result = await CSharpScript.EvaluateAsync(cleanedCode, evalOptions, Context, typeof(SocketCommandContext));
                    successCode = Result.GetType().Name;
                    evaluation = Result.ToString();
                    successOrError = new Color(75, 181, 67);

                }
                catch (Exception e)
                {

                    successCode = e.GetType().Name;
                    evaluation = e.Message;
                    successOrError = new Color(255, 71, 71);

                }

                _timer.Stop();

                var authorBuilder = new EmbedAuthorBuilder()
                {

                    Name = "Code Evaluation",
                    IconUrl = "http://crbtech.in/Dot-Net-Training/images/personal-traits-a-.net-developer.png",
                    Url = "https://github.com/dotnet/roslyn"

                };

                var footerBuilder = new EmbedFooterBuilder()
                {

                    Text = "Brought to you by Roslyn",
                    IconUrl = "http://jesse-raymond.com/images/CSharp-Logo.png"

                };

                eBuilder = new EmbedBuilder()
                {

                    Author = authorBuilder,
                    Color = successOrError,
                    ThumbnailUrl = "https://i.kinja-img.com/gawker-media/image/upload/s--c4SnNKka--/c_scale,f_auto,fl_progressive,q_80,w_800/fekfgutuwb84vdwujwv2.gif",
                    Footer = footerBuilder

                };

                eBuilder.AddField(x =>
                {

                    x.Name = "Code";
                    x.Value = $"```csharp\n{cleanedCode}```";

                });

                eBuilder.AddField(x =>
                {

                    x.Name = $"Result<{successCode}>";
                    x.Value = $"**Evaluated in {_timer.Elapsed}ms**\n```{evaluation}```";

                });

            }

            await ReplyAsync("", embed: eBuilder);

        }

        private string CleanCode(string rawCode)
        {

            string CleaningCode = rawCode;

            //it is 6 instead of 4 because there seems to be a problem where there are 2 more characters inserted after, no idea why
            if (CleaningCode.StartsWith("```")) CleaningCode = CleaningCode.Substring(3, CleaningCode.Length - 6);
            //4 instead of 2 because of same problem above
            else if (CleaningCode.StartsWith("`")) CleaningCode = CleaningCode.Substring(1, CleaningCode.Length - 4);

            if (CleaningCode.StartsWith("csharp")) CleaningCode = CleaningCode.Substring(5, CleaningCode.Length - 3);

            CleaningCode = CleaningCode.Trim();
            CleaningCode = CleaningCode.Replace(";", ";\n");

            return CleaningCode;

        }

    }
}
