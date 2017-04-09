using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YunoBotV2.Services.WebServices;

namespace YunoBotV2.Commands
{
    [Group("aqw")]
    public class AQWorlds : CustomModuleBase
    {

        private Web _service;

        public AQWorlds(Web serviceParams)
        {

            _service = serviceParams;

        }

        [Command(RunMode = RunMode.Async)]
        [Summary("Search AQWorlds wikidot")]
        public async Task AQWSearchCommand([Remainder]string search)
        {

            IHtmlDocument dom = await _service.GetDom($"http://aqwwiki.wikidot.com/search:site/a/p/q/{Uri.EscapeUriString(search)}");
            Dictionary<string, string> results = await GetResults(dom);

            if (results == null)
            {
                await DefaultErrorMessage();
                return;
            }

            var resultDict = new Dictionary<int, string>();
            var counter = 1;
            var organizedResults = new StringBuilder("```Here are your search results\n\n");

            foreach (string name in results.Keys)
            {

                organizedResults.AppendLine($"{counter}. {name}");
                resultDict.Add(counter, name);
                counter++;

            }

            organizedResults.Append("\nHit a number to get the link!```");
            await ReplyAsync(organizedResults.ToString());

            IUserMessage message = await WaitForMessage(Context.User, Context.Channel, TimeSpan.FromSeconds(60));

            if (int.TryParse(message.Content, out int userSearch))
                if (resultDict.TryGetValue(userSearch, out string name))
                    await ReplyAsync(results[name]);

        }

        private Task<Dictionary<string, string>> GetResults(IHtmlDocument dom)
        {

            IElement results = dom.GetElementsByClassName("search-results").First();

            if (results.TextContent.Contains("Sorry, no results found for your query."))
                return Task.FromResult<Dictionary<string, string>>(null);

            IHtmlCollection<IElement> items = results.GetElementsByClassName("item");
            var store = new Dictionary<string, string>();

            foreach (var item in items)
            {

                IElement title = item.GetElementsByClassName("title").First();
                string link = title.GetElementsByTagName("a").First().GetAttribute("href");
                string name = title.TextContent.Trim();
                store.Add(name, link);

            }

            return Task.FromResult(store);

        }
    }
}
