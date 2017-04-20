using AngleSharp.Dom.Html;
using Discord.Commands;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YunoBotV2.Commands.Attributes;
using YunoBotV2.Services.WebServices;

namespace YunoBotV2.Commands
{
    public class Wiki : CustomModuleBase
    {

        private Web _service;

        public Wiki(Web serviceParams)
            => _service = serviceParams;

        [Command("wiki")]
        [Summary("Search the wiki")]
        [Cooldown(5)]
        public async Task WikipediaCommand([Remainder]string search)
        {

            var url = $"https://en.wikipedia.org/w/api.php?action=opensearch&limit=1&namespace=0&format=json&redirects=resolve&search={Uri.EscapeUriString(search)}";

            using (Context.Channel.EnterTypingState())
            {
                var results = await _service.GetJArrayContent(url);

                //the results return in arrays inside an array.... why....
                //ex. bacteria will return
                // ["bacteria",["Bacteria"],["Bacteria (/b\u00e6k\u02c8t\u026a\u0259ri\u0259/; common noun bacteria, singular bacterium) constitute a large domain of prokaryotic microorganisms."],["https://en.wikipedia.org/wiki/Bacteria"]]
                //why is that even a thing...
                var token = results.ElementAtOrDefault(3)?.FirstOrDefault()?.Value<string>();

                if (token != null)
                    await ReplyAsync(token);
                else
                    await ReplyAsync("No results were returned!");
            }

        }

        [Command("wikia")]
        [Summary("Search the wikia")]
        public async Task WikiaCommand(string community, [Remainder]string search)
        {

            var url = $"http://{community}.wikia.com/wiki/Special:Search?search={Uri.EscapeUriString(search)}";
            var resultDict = new Dictionary<int, string>(10);

            using (Context.Channel.EnterTypingState())
            {

                IHtmlDocument dom = await _service.GetDom(url);
                var resultElement = dom.GetElementById("mw-content-text").GetElementsByClassName("Results").FirstOrDefault();

                if (resultElement == null)
                {
                    await NoResultsReturnedErrorMessage();
                    return;
                }

                var results = resultElement.Children;
                int max;

                if (results.Length >= 10)
                    max = 11;
                else
                    max = results.Length;

                var organizedResults = new StringBuilder("```Here are your results!\n\n");

                for (int counter = 1; counter < max; counter++) //only displaying 10 results
                {
                    var item = results[counter].GetElementsByTagName("a").First();

                    organizedResults.AppendLine($"{counter}. {item.TextContent}");
                    resultDict.Add(counter, item.GetAttribute("href"));
                }

                organizedResults.Append("\nHit a number to see that result!```");

                await ReplyAsync(organizedResults.ToString());

            }

            var message = await WaitForMessage(Context.User, Context.Channel, TimeSpan.FromSeconds(60));

            if (int.TryParse(message.Content, out int input))
                if (resultDict.TryGetValue(input, out string output))
                    await ReplyAsync(output);

        }

        [Command("gamepedia")]
        [Summary("Searches gamepedia")]
        public async Task GamepediaCommand(string game, [Remainder]string search)
        {

            var url = $"http://{game}.gamepedia.com/index.php?title=Special:Search&fulltext=Search&search={Uri.EscapeUriString(search)}";
            var resultDict = new Dictionary<int, string>(10);

            using (Context.Channel.EnterTypingState())
            {

                IHtmlDocument dom = await _service.GetDom(url);
                var resultElement = dom.GetElementById("mw-content-text").GetElementsByClassName("mw-search-results").FirstOrDefault();

                if (resultElement == null)
                {
                    await NoResultsReturnedErrorMessage();
                    return;
                }

                var results = resultElement.Children;
                int max;

                if (results.Length >= 10)
                    max = 11;
                else
                    max = results.Length;

                var organizedResults = new StringBuilder("```Here are your results!\n\n");

                for (int counter = 1; counter < max; counter++) //only displaying 10 results
                {
                    var item = results[counter].GetElementsByTagName("a").First();

                    organizedResults.AppendLine($"{counter}. {item.GetAttribute("title")}");
                    resultDict.Add(counter, $"http://{game}.gamepedia.com{item.GetAttribute("href")}");
                }

                organizedResults.Append("\nHit a number to see that result!```");

                await ReplyAsync(organizedResults.ToString());

            }

            var message = await WaitForMessage(Context.User, Context.Channel, TimeSpan.FromSeconds(60));

            if (int.TryParse(message.Content, out int input))
                if (resultDict.TryGetValue(input, out string output))
                    await ReplyAsync(output);

        }

    }
}
