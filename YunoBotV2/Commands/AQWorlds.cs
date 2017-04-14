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

        [Command("user")]
        [Summary("Search an AQWorlds user")]
        public async Task AQWUserCommand([Remainder]string user)
        {

            using (Context.Channel.EnterTypingState())
            {

                var url = $"http://www.aq.com/character.asp?id={Uri.EscapeUriString(user)}";
                IHtmlDocument dom = await _service.GetDom(url);

                if (dom.GetElementById("character5") == null)
                    await NoResultsReturnedErrorMessage();

                IHtmlCollection<IElement> achievementNodes = dom.GetElementsByClassName("achievements").FirstOrDefault()?.GetElementsByTagName("a");
                var achievements = new List<string>(15); //achievements

                if (achievementNodes != null)
                {
                    foreach (var achievement in achievementNodes)
                    {
                        achievements.Add(achievement.GetAttribute("title"));
                    }
                }

                IHtmlCollection<IElement> inventoryNodes = dom.GetElementsByClassName("row items").FirstOrDefault()?.GetElementsByTagName("div").FirstOrDefault()?.GetElementsByClassName("item");
                var inventory = new List<string>(20); //inventory

                if (inventoryNodes != null)
                {
                    foreach (var item in inventoryNodes)
                    {
                        inventory.Add(item.GetElementsByTagName("div").FirstOrDefault()?.TextContent);
                    }
                }

                IHtmlCollection<IElement> classNodes = dom.GetElementsByClassName("row items").FirstOrDefault()?.GetElementsByClassName("col-md-6 col-sm-6 col-xs-12")[1].GetElementsByClassName("item");
                var classes = new List<string>(20); //classes and armors

                if (classNodes != null)
                {
                    foreach (var @class in classNodes)
                    {
                        classes.Add(@class.GetElementsByTagName("div").FirstOrDefault()?.TextContent);
                    }
                }

                IHtmlCollection<IElement> houseNodes = dom.GetElementsByClassName("row items")[1].GetElementsByClassName("col-md-6 col-sm-6 col-xs-12");
                var houseItems = new List<string>(20); //house items

                if (houseNodes != null)
                {
                    foreach (var houseStuff in houseNodes)
                    {
                        foreach (var item in houseStuff.GetElementsByClassName("item"))
                        {
                            houseItems.Add(item.GetElementsByTagName("div").FirstOrDefault()?.TextContent);
                        }
                    }
                }

                await ReplyAsync($"Dumping information on AQWorlds player {user}...");

                var organizedAchievements = new StringBuilder("```\nAchievements\n\n");
                achievements.ForEach(achievement =>
                {
                    if (achievement != null)
                        organizedAchievements.AppendLine(achievement.Trim());
                });
                organizedAchievements.Append("```");

                var organizedInventory = new StringBuilder("```\nInventory\n\n");
                inventory.ForEach(item =>
                {
                    if (item != null)
                        organizedInventory.AppendLine(item.Trim());
                });
                organizedInventory.Append("```");

                var organizedClasses = new StringBuilder("```Classes and Armors\n\n");
                classes.ForEach(@class =>
                {
                    if (@class != null)
                        organizedClasses.AppendLine(@class.Trim());
                });
                organizedClasses.Append("```");

                var organizedHouse = new StringBuilder("```\nHouse Items\n\n");
                houseItems.ForEach(item =>
                {
                    if (item != null)
                        organizedHouse.AppendLine(item.Trim());
                });
                organizedHouse.Append("```");

                await ReplyAsync(organizedAchievements.ToString());
                await ReplyAsync(organizedInventory.ToString());
                await ReplyAsync(organizedClasses.ToString());
                await ReplyAsync(organizedHouse.ToString());

            }

        }

        [Command("search")]
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
