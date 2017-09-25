﻿using AngleSharp.Dom;
using Discord;
using Discord.Commands;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YunoV3.Services;

namespace YunoV3.Modules.Commands
{
    public class WebRelated : CustomBase
    {

        private Web _web;
        private Random _random;

        public WebRelated(Web web, Random random)
        {

            _web = web;
            _random = random;

        }

        [Command("needsmorejpeg")]
        [Summary("NEEDS MOAR JPEG")]
        public async Task NeedsMoreJpeg()
        {

            var messages = await Context.Channel.GetMessagesAsync(50).Flatten();
            var message = messages.FirstOrDefault(m => m.Attachments.Any(attachment => attachment.Height != null));

            if (message != null)
            {

                var attachment = message.Attachments.FirstOrDefault(a => a.Height != null);

                if (attachment != null)
                {

                    var result = await _web.GetStreamAsync(attachment.Url);

                    using (var stream = result.stream)
                    {

                        var image = SixLabors.ImageSharp.Image.Load(stream);

                        using (var payload = new MemoryStream())
                        {

                            var encoder = new JpegEncoder()
                            {

                                Quality = 1,
                                IgnoreMetadata = true,
                                Subsample = JpegSubsample.Ratio420

                            };

                            image.SaveAsJpeg(payload, encoder);

                            payload.Seek(0, SeekOrigin.Begin);

                            await UploadAsync(payload, attachment.Filename);

                        }

                    }

                }

            }
            else
                await ReplyAsync("There are no images to meme in recent messages.");

        }

        [Command("garfield")]
        [Summary("Get a random garfield comic")]
        public async Task GetRandomGarfield()
        {

            IElement element;

            do
            {

                //garfield began in 1978 but in the middle of that year and i aint
                //checking whether or not that day of the year is valid
                var year = _random.Next(1979, DateTime.Now.Year + 1);
                var month = _random.Next(1, 13);
                //i aint checking for leap year either
                var day = _random.Next(1, 29);
                var url = $"https://garfield.com/comic/{year}/{month}/{day}";
                var dom = await _web.GetDomAsync(url);
                element = dom.GetElementsByClassName("comic-display").First()
                    .GetElementsByClassName("img-responsive").FirstOrDefault();

            } while (element == null);

            var image = element.GetAttribute("src");
            var result = await _web.GetStreamAsync(image);            
            
            await UploadAsync(result.stream, new Uri(image).Segments.Last().Replace("gif", "png"));

        }

        [Command("gif")]
        [Summary("Get a gif from giphy!")]
        public async Task GetAGifFromGiphy([Remainder]string topic = null)
        {

            //api key is the public one, i dont request from it too much to use a legit one anyway
            var url = "http://api.giphy.com/v1/gifs/random?api_key=dc6zaTOxFJmzC";

            if (!string.IsNullOrEmpty(topic))
                url += $"&tag={Uri.EscapeUriString(topic)}";

            var result = await _web.GetJObjectAsync(url);
            var gifUrl = result["data"]["image_original_url"].ToString();
            var downData = await _web.GetStreamAsync(gifUrl);

            await UploadAsync(downData.stream, downData.filename ?? "giphy.gif");

        }

    }
}