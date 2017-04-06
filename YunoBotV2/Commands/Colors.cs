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

                if(token == null)
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
                    Color = new Color(byte.Parse(result.rgb.red), byte.Parse(result.rgb.green), byte.Parse(result.rgb.blue)),
                    Title = result.title,
                    ThumbnailUrl = result.imageUrl,
                    Url = result.url,
                    Footer = footerBuilder

                };

                eBuilder.AddField(x =>
                {
                    x.Name = "Hex Code";
                    x.Value = result.hex;
                });

                eBuilder.AddField(x =>
                {
                    x.Name = "RGB Value";
                    x.Value = $"{result.rgb.red}, {result.rgb.green}, {result.rgb.blue}";
                });

                eBuilder.AddField(x =>
                {
                    x.Name = "Hue";
                    x.Value = result.hsv.hue;
                });

                eBuilder.AddField(x =>
                {
                    x.Name = "Saturation";
                    x.Value = result.hsv.saturation;
                });

                await ReplyAsync("", embed: eBuilder);

            }

        }

        [Command]
        [Summary("Gets color with given hexcode")]
        public async Task HexcodeCommand(string hexcode)
        {

            using (Context.Channel.EnterTypingState())
            {

                var url = $"http://www.colourlovers.com/api/color/{hexcode.Replace("#", "")}?format=json";
                JToken token = await _service.GetFirstJArrayContent(url);

                if(token == null)
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
                    Color = new Color(byte.Parse(result.rgb.red), byte.Parse(result.rgb.green), byte.Parse(result.rgb.blue)),
                    Title = result.title,
                    ThumbnailUrl = result.imageUrl,
                    Url = result.url,
                    Footer = footerBuilder

                };

                eBuilder.AddField(x =>
                {
                    x.Name = "Hex Code";
                    x.Value = result.hex;
                });

                eBuilder.AddField(x =>
                {
                    x.Name = "RGB Value";
                    x.Value = $"{result.rgb.red}, {result.rgb.green}, {result.rgb.blue}";
                });

                eBuilder.AddField(x =>
                {
                    x.Name = "Hue";
                    x.Value = result.hsv.hue;
                });

                eBuilder.AddField(x =>
                {
                    x.Name = "Saturation";
                    x.Value = result.hsv.saturation;
                });

                await ReplyAsync("", embed: eBuilder);

            }

        }

    }
}
