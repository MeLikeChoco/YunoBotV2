using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YunoBotV2.Objects.Deserializers;
using YunoBotV2.Services.WebServices;

namespace YunoBotV2.Commands
{
    //[Name("Colors")]
    [Group("color")]
    public class Colors : CustomModuleBase
    {

        public Web _service { get; set; }

        //public Colors(Web serviceParams)
        //{

        //    _service = serviceParams;

        //}

        [Command]
        [Summary("Gets color with given hexcode")]
        public async Task ColorCommand(string hexcode = "")
        {

            string url;

            if (string.IsNullOrEmpty(hexcode))
                url = "http://www.colourlovers.com/api/colors/random?format=json";
            else
                url = $"http://www.colourlovers.com/api/color/{hexcode.Replace("#", "")}?format=json";

            using (Context.Channel.EnterTypingState())
            {

                JToken token = await _service.GetFirstJArrayContent(url);

                if (token == null)
                {
                    await DefaultErrorMessage();
                    return;
                }

                ColorLouver result = token.ToObject<ColorLouver>();

                var author = new EmbedAuthorBuilder()
                    .WithName("Colour Lovers")
                    .WithUrl("http://www.colourlovers.com/")
                    .WithIconUrl("http://icons.iconarchive.com/icons/alecive/flatwoken/256/Apps-Color-B-icon.png");

                var body = new EmbedBuilder()
                {

                    Author = author,
                    Color = new Color(int.Parse(result.RGB.Red), int.Parse(result.RGB.Green), int.Parse(result.RGB.Blue)),
                    Title = result.Title,
                    ThumbnailUrl = result.ImageUrl,
                    Url = result.Url,

                };

                body.AddField("Hexcode", result.Hex);
                body.AddField("RGB Value", $"{result.RGB.Red}, {result.RGB.Green}, {result.RGB.Blue}");
                body.AddField("Hue", result.HSV.Hue);
                body.AddField("Saturation", result.HSV.Saturation);

                await ReplyAsync("", embed: body);

            }

        }

        [Command("search")]
        [Summary("Searches for colors based on keywords")]
        public async Task ColorSearchCommand([Remainder]string search = "")
        {

            if (string.IsNullOrEmpty(search))
            {

                await SearchErrorMessage();
                return;

            }

            var url = $"http://www.colourlovers.com/api/colors?keywords={search}&format=json&numResults=100";

            using (Context.Channel.EnterTypingState())
            {

                JArray results = await _service.GetJArrayContent(url);

                if(results == null)
                {
                    await DefaultErrorMessage();
                    return;
                }

                int resultsShown = results.Count > 21 ? 20 : results.Count;
                var organizedResults = new StringBuilder($"```Showing {resultsShown}/{results.Count} results\n\n");

                for (int i = 0; i < resultsShown; i++)
                {

                    var temp = results[i];
                    organizedResults.AppendLine($"{i + 1}: {temp.Value<string>("title")}");

                }

                organizedResults.Append("\nHit a corresponding number to see that color!```");

                await ReplyAsync(organizedResults.ToString());

                var message = await WaitForMessage(Context.User, Context.Channel, TimeSpan.FromSeconds(60));

                if (int.TryParse(message.Content, out var choice) && choice > 0 && choice < 21)
                    await ColorCommand(results[--choice].Value<string>("hexcode"));
            }

        }

    }
}
