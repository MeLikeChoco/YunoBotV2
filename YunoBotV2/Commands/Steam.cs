using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YunoBotV2.Services.WebServices;
using AngleSharp.Dom.Html;
using AngleSharp.Dom;
using YunoBotV2.Services;
using Discord;
using System.Net;
using YunoBotV2.Objects;

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

                List<SteamFrontpageObject> specials;

                if (DateTime.Now.Subtract(Cache.LastSteamSpecialScrape) > TimeSpan.FromMinutes(60))
                {
                    specials = await GetSteamGames(url, ScrapeType.Specials);
                }
                else
                {
                    await ReplyAsync("", embed: Cache.SteamSpecials);
                    return;
                }

                if (specials == null)
                {
                    await DefaultErrorMessage();
                    return;
                }

                var body = StartEmbed(ScrapeType.Specials, specials.FirstOrDefault().Picture);
                var counter = 1;

                foreach (SteamFrontpageObject game in specials)
                {

                    body.AddField(x =>
                    {
                        x.Name = $"{counter}. {WebUtility.HtmlDecode(game.Title)}";
                        x.Value = $"Rating: {game.Rating}\nRelease Date: {game.ReleaseDate}\nOriginal Price: {game.OgPrice}\nNew Price: {game.NewPrice}\nDiscount: {game.Discount}\n[Link]({game.Link})";
                    });

                    counter++;

                }

                await ReplyAsync("", embed: body);
                Cache.SteamSpecials = body;

            }

        }

        [Command("top")]
        [Summary("Return top 10 best sellers")]
        public async Task SteamTopCommand()
        {

            var url = "http://store.steampowered.com/search/?filter=globaltopsellers";

            using (Context.Channel.EnterTypingState())
            {

                List<SteamFrontpageObject> topSellers;

                if (DateTime.Now.Subtract(Cache.LastSteamTopSellerScrape) > TimeSpan.FromMinutes(60))
                {
                    topSellers = await GetSteamGames(url, ScrapeType.TopSellers);
                }
                else
                {
                    await ReplyAsync("", embed: Cache.SteamTopSellers);
                    return;
                }

                if (topSellers == null)
                {
                    await DefaultErrorMessage();
                    return;
                }

                var body = StartEmbed(ScrapeType.TopSellers, topSellers.FirstOrDefault().Picture);
                var counter = 1;

                foreach (SteamFrontpageObject game in topSellers)
                {

                    body.AddField(x =>
                    {
                        x.Name = $"{counter}. {WebUtility.HtmlDecode(game.Title)}";
                        x.Value = $"Rating: {game.Rating}\nRelease Date: {game.ReleaseDate}\nOriginal Price: {game.OgPrice}\nNew Price: {game.NewPrice}\nDiscount: {game.Discount}\n[Link]({game.Link})";
                    });

                    counter++;

                }

                await ReplyAsync("", embed: body);
                Cache.SteamTopSellers = body;

            }

        }

        [Command("new")]
        [Summary("Return the top 10 new releases")]
        public async Task SteamNewCommand()
        {

            var url = "http://store.steampowered.com/search/?filter=popularnew&sort_by=Released_DESC";

            using (Context.Channel.EnterTypingState())
            {

                List<SteamFrontpageObject> topNewReleases;

                if (DateTime.Now.Subtract(Cache.LastSteamNewReleasesScrape) > TimeSpan.FromMinutes(60))
                {
                    topNewReleases = await GetSteamGames(url, ScrapeType.New);
                }
                else
                {
                    await ReplyAsync("", embed: Cache.SteamNewReleases);
                    return;
                }

                if (topNewReleases == null)
                {
                    await DefaultErrorMessage();
                    return;
                }

                var body = StartEmbed(ScrapeType.New, topNewReleases.FirstOrDefault().Picture);
                var counter = 1;

                foreach (SteamFrontpageObject game in topNewReleases)
                {

                    body.AddField(x =>
                    {
                        x.Name = $"{counter}. {WebUtility.HtmlDecode(game.Title)}";
                        x.Value = $"Rating: {game.Rating}\nRelease Date: {game.ReleaseDate}\nOriginal Price: {game.OgPrice}\nNew Price: {game.NewPrice}\nDiscount: {game.Discount}\n[Link]({game.Link})";
                    });

                    counter++;

                }

                await ReplyAsync("", embed: body);
                Cache.SteamNewReleases = body;

            }

        }

        private EmbedBuilder StartEmbed(ScrapeType scrapeType, string thumbnail)
        {

            string title, url;
            DateTime scrapeTime;

            switch (scrapeType)
            {

                case ScrapeType.New:
                    title = "Frontpage Steam New Releases";
                    scrapeTime = Cache.LastSteamNewReleasesScrape;
                    url = "http://store.steampowered.com/search/?filter=popularnew&sort_by=Released_DESC";
                    break;
                case ScrapeType.Specials:
                    title = "Frontpage Steam Specials";
                    scrapeTime = Cache.LastSteamSpecialScrape;
                    url = "http://store.steampowered.com/search/?specials=1";
                    break;
                case ScrapeType.TopSellers:
                    title = "Frontpage Steam Top Sellers";
                    scrapeTime = Cache.LastSteamTopSellerScrape;
                    url = "http://store.steampowered.com/search/?filter=globaltopsellers";
                    break;
                default:
                    title = "Frontpage Steam Specials";
                    scrapeTime = Cache.LastSteamSpecialScrape;
                    url = "http://store.steampowered.com/search/?specials=1";
                    break;


            }

            var author = new EmbedAuthorBuilder()
                .WithIconUrl("http://icons.iconarchive.com/icons/martz90/circle/256/steam-icon.png")
                .WithName("Steam")
                .WithUrl("http://store.steampowered.com/");

            var footer = new EmbedFooterBuilder()
                .WithText($"Valve© | Data Last Gathered: {scrapeTime.ToString("MM/dd/yyyy hh:mm")}")
                .WithIconUrl("http://icons.iconarchive.com/icons/bokehlicia/pacifica/256/steam-icon.png");

            var body = new EmbedBuilder()
            {

                Author = author,
                Color = new Color(27, 40, 56),
                Title = title,
                Url = url,
                Description = "Here are the top 10 new releases on the front page of steam",
                ThumbnailUrl = thumbnail,
                Footer = footer

            };

            return body;

        }

        private async Task<List<SteamFrontpageObject>> GetSteamGames(string url, ScrapeType scrapeType)
        {

            List<SteamFrontpageObject> specials = new List<SteamFrontpageObject>(10);
            IHtmlDocument dom = await _service.GetDom(url);

            if (dom == null) return null;

            Console.WriteLine(dom.TextContent);

            //its index 1 because there's a random div before the container i want
            IEnumerable<IElement> elements = dom.GetElementById("search_result_container").GetElementsByTagName("div")[1].GetElementsByTagName("a").Take(10);

            foreach (var element in elements)
            {

                specials.Add(new SteamFrontpageObject
                {

                    Link = element.GetAttribute("href"),
                    Picture = element.GetElementsByClassName("col search_capsule").First().FirstElementChild.GetAttribute("src"),
                    Title = element.GetElementsByClassName("title").First().TextContent,
                    ReleaseDate = element.GetElementsByClassName("col search_released responsive_secondrow").FirstOrDefault()?.TextContent ?? "No Release Date",
                    Rating = element.GetElementsByClassName("col search_reviewscore responsive_secondrow").FirstOrDefault()?.FirstElementChild?.GetAttribute("data-store-tooltip")?.Replace("<br>", ", ") ?? "No ratings",
                    Discount = element.GetElementsByClassName("col search_discount responsive_secondrow").FirstOrDefault()?.FirstElementChild?.TextContent.Replace("-", string.Empty) ?? "No discount",
                    OgPrice = element.GetElementsByClassName("col search_price discounted responsive_secondrow").FirstOrDefault()?.GetElementsByTagName("strike").FirstOrDefault()?.TextContent ?? element.GetElementsByClassName("col search_price  responsive_secondrow").FirstOrDefault().TextContent.Trim(),
                    NewPrice = element.GetElementsByClassName("col search_price discounted responsive_secondrow").FirstOrDefault()?.ChildNodes[3]?.TextContent ?? "N/A",

                });

            }

            if (scrapeType == ScrapeType.Specials)
                Cache.LastSteamSpecialScrape = DateTime.Now;
            else if (scrapeType == ScrapeType.TopSellers)
                Cache.LastSteamTopSellerScrape = DateTime.Now;
            else if (scrapeType == ScrapeType.New)
                Cache.LastSteamNewReleasesScrape = DateTime.Now;

            return specials;

        }

        private enum ScrapeType
        {
            Specials,
            TopSellers,
            New
        }

    }
}
