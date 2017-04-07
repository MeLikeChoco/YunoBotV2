using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using YunoBotV2.Deserializers;
using YunoBotV2.Services.WebServices;
using AngleSharp.Dom.Html;
using AngleSharp.Dom;
using YunoBotV2.Services;
using Discord;
using System.Net;

namespace YunoBotV2.Commands
{
    [Group("steam")]
    public class Steam : CustomModuleBase
    {

        private Web _service;

        public Steam(Web serviceParams)
        {

            _service = serviceParams;

        }

        [Command("deals")]
        [Summary("Returns the top 10 specials")]
        public async Task DealsCommand()
        {

            var url = "http://store.steampowered.com/search/?specials=1";

            using (Context.Channel.EnterTypingState())
            {

                List<SteamSpecialObject> specials;

                if (DateTime.Now.Subtract(Cache.LastSteamSpecialScrape) > TimeSpan.FromMinutes(60))
                {
                    specials = await GetSteamSpecials(url);
                }
                else
                {
                    specials = Cache.SteamSpecials;
                }

                if (specials == null)
                {
                    await DefaultErrorMessage();
                    return;
                }

                var AuthorBuilder = new EmbedAuthorBuilder()
                {

                    Name = "Steam",
                    Url = "http://store.steampowered.com/",
                    IconUrl = "http://icons.iconarchive.com/icons/martz90/circle/256/steam-icon.png"

                };

                var FooterBuilder = new EmbedFooterBuilder()
                {

                    Text = $"Valve© | Data Last Gathered: {Cache.LastSteamSpecialScrape.ToString("MM/dd/yyyy hh:mm")}",
                    IconUrl = "http://icons.iconarchive.com/icons/bokehlicia/pacifica/256/steam-icon.png"

                };

                var eBuilder = new EmbedBuilder()
                {

                    Author = AuthorBuilder,
                    Color = new Color(27, 40, 56),
                    Title = "Frontpage Steam Specials",
                    Url = "http://store.steampowered.com/search/?specials=1",
                    Description = "Here are the top 10 deals on the front page of steam",
                    ThumbnailUrl = specials.FirstOrDefault().Picture,
                    Footer = FooterBuilder

                };

                var counter = 1;

                foreach (SteamSpecialObject game in specials)
                {

                    eBuilder.AddField(x =>
                    {
                        x.Name = $"{counter}. {WebUtility.HtmlDecode(game.Title)}";
                        x.Value = $"Rating: {game.Rating}\nRelease Date: {game.ReleaseDate}\nOriginal Price: {game.OgPrice}\nNew Price: {game.NewPrice}\nDiscount: {game.Discount}\n[Link]({game.Link})";
                    });

                    counter++;

                }

                await ReplyAsync("", embed: eBuilder);

            }

        }

        private async Task<List<SteamSpecialObject>> GetSteamSpecials(string url)
        {

            List<SteamSpecialObject> specials = new List<SteamSpecialObject>(10);
            IHtmlDocument dom = await _service.GetDom(url);

            if (dom == null) return null;

            Console.WriteLine(dom.TextContent);

            //its index 1 because there's a random div before the container i want
            IEnumerable<IElement> elements = dom.GetElementById("search_result_container").GetElementsByTagName("div")[1].GetElementsByTagName("a").Take(10);

            foreach(var element in elements)
            {

                specials.Add(new SteamSpecialObject
                {

                    Link = element.GetAttribute("href"),
                    Picture = element.GetElementsByClassName("col search_capsule").First().FirstElementChild.GetAttribute("src"),
                    Title = element.GetElementsByClassName("title").First().TextContent,
                    ReleaseDate = element.GetElementsByClassName("col search_released responsive_secondrow").First().TextContent ?? "No Release Date",
                    Rating = element.GetElementsByClassName("col search_reviewscore responsive_secondrow").First().FirstElementChild.GetAttribute("data-store-tooltip").Replace("<br>", ", ") ?? "N/A",
                    Discount = element.GetElementsByClassName("col search_discount responsive_secondrow").First().FirstElementChild.TextContent.Replace("-", string.Empty),
                    OgPrice = element.GetElementsByClassName("col search_price discounted responsive_secondrow").First().GetElementsByTagName("strike").First().TextContent,
                    NewPrice = element.GetElementsByClassName("col search_price discounted responsive_secondrow").First().ChildNodes[3].TextContent,

                });

            }

            Cache.SteamSpecials = specials;
            Cache.LastSteamSpecialScrape = DateTime.Now;

            return specials;

        }

    }
}
