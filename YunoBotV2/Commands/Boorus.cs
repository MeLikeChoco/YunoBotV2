using Discord.Commands;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YunoBotV2.Services;
using YunoBotV2.Services.WebServices;

namespace YunoBotV2.Commands
{
    [Name("NSFW/SFW")]
    public class Boorus : CustomModuleBase
    {

        private Web _web;
        private static readonly string[] SiteList = new string[]
        {

            "https://gelbooru.com/",
            "http://rule34.xxx/"

        };

        public Boorus(Web serviceParams)
        {

            _web = serviceParams;

        }

        [Command("nsfw")]
        [Summary("Returns a nsfw picture")]
        [RequireNsfw]
        public async Task NsfwCommand([Remainder]string tags = "")
        {

            tags = tags.ToLower();

            if (tags.Contains("webm"))
            {
                await ReplyAsync("No point in webm, they don't display anyway.");
                return;
            }

            if (tags.Substring(0, 4) == "real")
            {

                await SendReal(tags.Substring(4));

            }
            else
                await SendHentai(tags, true);

        }

        [Command("sfw")]
        [Summary("Returns a sfw picture")]
        public async Task SfwCommand([Remainder]string tags = "")
        {

            if (tags.Contains("webm"))
            {
                await ReplyAsync("No point in webm, they don't display anyway.");
                return;
            }

            await SendHentai(tags, false);

        }

        public async Task SendReal(string tags)
        {

            using (Context.Channel.EnterTypingState())
            {

                var baseUrl = "http://rb.booru.org/";
                var url = $"{baseUrl}index.php?page=post&s=list&tags={Uri.EscapeUriString(tags)}";
                var dom = await _web.GetDom(url);
                var paginator = dom.GetElementById("paginator");

                if (paginator != null)
                {

                    var pages = paginator.Children
                        .Where(element => element.TagName == "A" && !element.HasAttribute("alt"))
                        .Select(element => element.GetAttribute("href"));
                    var pageCount = pages.Count();
                    var pIndex = Rand.Next(pageCount + 1);

                    if(pIndex != pageCount + 1)
                        url = baseUrl + pages.ElementAt(pIndex);

                    dom = await _web.GetDom(url);

                }
                
                var results = dom.GetElementsByClassName("thumb");
                var picNum = results.Count();

                if (picNum == 0)
                {

                    await NoResultsReturnedErrorMessage();
                    return;

                }

                var index = Rand.Next(picNum);
                var picture = results[index];
                url = baseUrl + picture.FirstElementChild.GetAttribute("href");
                dom = await _web.GetDom(url);
                picture = dom.GetElementById("note-container").FirstElementChild;

                await ReplyAsync(picture.GetAttribute("src"));

            }

        }

        public async Task SendHentai(string tags, bool nsfw)
        {

            using (Context.Channel.EnterTypingState())
            {

                var booru = Rand.Next(SiteList.Length);

                var url = nsfw ?
                $"{SiteList[booru]}index.php?page=dapi&s=post&q=index&tags={Uri.EscapeUriString(tags)}%20{Uri.EscapeUriString("-webm")}%20rating:explicit&limit=100&json=1" :
                $"{SiteList[booru]}index.php?page=dapi&s=post&q=index&tags={Uri.EscapeUriString(tags)}%20{Uri.EscapeUriString("-webm")}%20rating:safe&limit=100&json=1";
                JArray searchResults = await _web.GetJArrayContent(url);

                if (searchResults == null)
                {
                    await DefaultErrorMessage();
                    return;
                }
                else if (searchResults.Count == 0)
                {
                    await ReplyAsync($"{Context.User.Mention} ```Use http://gelbooru.com/index.php?page=tags&s=list to get a list of accepted tags```" +
                        "\n```Use http://gelbooru.com/index.php?page=help&topic=cheatsheet for additional tag formatting```");
                    return;
                }

                string fileUrl;

                do
                {

                    int i = Rand.Next(0, searchResults.Count);
                    fileUrl = searchResults[i].Value<string>("file_url");

                } while (fileUrl.Contains(".webm"));

                await ReplyAsync($"http:{fileUrl}");

            }

        }

    }
}
