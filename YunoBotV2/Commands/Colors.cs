using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YunoBotV2.Deserializers;
using YunoBotV2.Services.WebServices;

namespace YunoBotV2.Commands
{
    [Group("color")]
    public class Colors : CustomModuleBase
    {

        private Web _service;

        public Colors(Web serviceParams)
        {

            _service = serviceParams;

        }

        [Command]
        [Summary("Returns a random color")]
        public async Task ColorCommand()
        {

            using (Context.Channel.EnterTypingState())
            {

                var url = "http://www.colourlovers.com/api/colors/random?format=json";
                JToken token = await _service.GetFirstJArrayContent(url);

                if (token == null)
                {
                    await DefaultErrorMessage();
                    return;
                }

                ColorLouver result = token.ToObject<ColorLouver>();

                var authorBuilder = new EmbedAuthorBuilder()
                {

                    Name = "Colour Lovers",
                    Url = "http://www.colourlovers.com/",
                    IconUrl = "http://icons.iconarchive.com/icons/alecive/flatwoken/256/Apps-Color-B-icon.png"

                };

                var footerBuilder = new EmbedFooterBuilder()
                {

                    Text = $"Poop has color too | {(await Context.Client.GetApplicationInfoAsync()).Owner}",
                    IconUrl = "http://icons.iconarchive.com/icons/iconshock/brilliant-graphics/256/colors-icon.png"

                };

                EmbedBuilder eBuilder = new EmbedBuilder()
                {

                    Author = authorBuilder,
                    Color = new Color(byte.Parse(result.RGB.Red), byte.Parse(result.RGB.Green), byte.Parse(result.RGB.Blue)),
                    Title = result.Title,
                    ThumbnailUrl = result.ImageUrl,
                    Url = result.Url,
                    Footer = footerBuilder

                };

                eBuilder.AddField(x =>
                {
                    x.Name = "Hex Code";
                    x.Value = result.Hex;
                });

                eBuilder.AddField(x =>
                {
                    x.Name = "RGB Value";
                    x.Value = $"{result.RGB.Red}, {result.RGB.Green}, {result.RGB.Blue}";
                });

                eBuilder.AddField(x =>
                {
                    x.Name = "Hue";
                    x.Value = result.HSV.Hue;
                });

                eBuilder.AddField(x =>
                {
                    x.Name = "Saturation";
                    x.Value = result.HSV.Saturation;
                });

                await ReplyAsync("", embed: eBuilder);

            }

        }

        [Command("search", RunMode = RunMode.Async)]
        [Summary("Searches for colors based on keywords")]
        public async Task ColorSearchCommand([Remainder]string search = "")
        {

            if (string.IsNullOrEmpty(search))
            {
                await SearchErrorMessage();
                return;
            }

            var url = $"http://www.colourlovers.com/api/colors?keywords={search}&format=json&numResults=100";
            var searchResults = new Dictionary<int, string>(20);

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
                    searchResults.Add(i + 1, temp.Value<string>("hex"));

                }

                organizedResults.Append("\nHit a corresponding number to see that color!```");

                await ReplyAsync(organizedResults.ToString());

                IMessage message = await WaitForMessage(Context.User, Context.Channel, TimeSpan.FromSeconds(60));

                if (int.TryParse(message.Content, out int color))
                {
                    if (searchResults.TryGetValue(color, out string hexcode))
                        await HexcodeCommand(hexcode);
                }
            }

        }

        [Command]
        [Summary("Gets color with given hexcode")]
        public async Task HexcodeCommand(string hexcode = "")
        {

            if (string.IsNullOrEmpty(hexcode))
                await SearchErrorMessage();

            using (Context.Channel.EnterTypingState())
            {

                var url = $"http://www.colourlovers.com/api/color/{hexcode.Replace("#", "")}?format=json";
                JToken token = await _service.GetFirstJArrayContent(url);

                if (token == null)
                {
                    await DefaultErrorMessage();
                    return;
                }

                ColorLouver result = token.ToObject<ColorLouver>();

                var authorBuilder = new EmbedAuthorBuilder()
                {

                    Name = "Colour Lovers",
                    Url = "http://www.colourlovers.com/",
                    IconUrl = "http://icons.iconarchive.com/icons/alecive/flatwoken/256/Apps-Color-B-icon.png"

                };

                var footerBuilder = new EmbedFooterBuilder()
                {

                    Text = $"Poop has color too | {(await Context.Client.GetApplicationInfoAsync()).Owner}",
                    IconUrl = "http://icons.iconarchive.com/icons/iconshock/brilliant-graphics/256/colors-icon.png"

                };

                EmbedBuilder eBuilder = new EmbedBuilder()
                {

                    Author = authorBuilder,
                    Color = new Color(byte.Parse(result.RGB.Red), byte.Parse(result.RGB.Green), byte.Parse(result.RGB.Blue)),
                    Title = result.Title,
                    ThumbnailUrl = result.ImageUrl,
                    Url = result.Url,
                    Footer = footerBuilder

                };

                eBuilder.AddField(x =>
                {
                    x.Name = "Hex Code";
                    x.Value = result.Hex;
                });

                eBuilder.AddField(x =>
                {
                    x.Name = "RGB Value";
                    x.Value = $"{result.RGB.Red}, {result.RGB.Green}, {result.RGB.Blue}";
                });

                eBuilder.AddField(x =>
                {
                    x.Name = "Hue";
                    x.Value = result.HSV.Hue;
                });

                eBuilder.AddField(x =>
                {
                    x.Name = "Saturation";
                    x.Value = result.HSV.Saturation;
                });

                await ReplyAsync("", embed: eBuilder);

            }

        }

    }
}
