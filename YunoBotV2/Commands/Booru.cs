using Discord.Commands;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YunoBotV2.Services.WebServices;

namespace YunoBotV2.Commands
{
    public class Booru : CustomModuleBase
    {

        private WebService _service;

        public Booru(WebService serviceParams)
        {

            _service = serviceParams;

        }

        [Command("nsfw")]
        [Summary("Returns a nsfw picture")]
        public async Task NsfwCommand([Remainder]string tags = "")
        {

            //temporary measure against accidental nsfw pictures
            if (!Context.Channel.Name.ToLower().Contains("nsfw")) return;

            if (tags.Contains("webm"))
            {
                await ReplyAsync("No point in webm, they don't display anyway.");
                return;
            }

            await Gelbooru(tags, true);

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

            await Gelbooru(tags, false);

        }

        public async Task Gelbooru(string tags, bool nsfw)
        {

            using (Context.Channel.EnterTypingState())
            {

                var url = nsfw ?
                $"http://gelbooru.com/index.php?page=dapi&s=post&q=index&tags={Uri.EscapeUriString(tags)}%20rating:explicit&limit=1000&json=1" :
                $"http://gelbooru.com/index.php?page=dapi&s=post&q=index&tags={Uri.EscapeUriString(tags)}%20rating:safe&limit=1000&json=1";
                JArray searchResults = await _service.GetJArrayContent(url);

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

                Random r = new Random();
                string fileUrl;

                do
                {

                    int i = r.Next(0, searchResults.Count);
                    fileUrl = searchResults[i].Value<string>("file_url");

                } while (fileUrl.Contains(".webm"));

                await ReplyAsync($"http:{fileUrl}");

            }

        }

    }
}
