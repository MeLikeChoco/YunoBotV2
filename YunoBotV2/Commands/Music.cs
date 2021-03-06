﻿using Discord.Commands;
using Discord.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YunoBotV2.Services.WebServices;

namespace YunoBotV2.Commands
{
    public class Music : CustomModuleBase
    {

        private Web _web;

        private const string LyricsSearch = "http://search.azlyrics.com/search.php?q=";
        private const string NextLinePattern = @"\s*<br>";
        private const string HtmlTagPattern = @"<.*?>";

        public Music(Web web)
        {

            _web = web;

        }

        [Command("lyrics")]
        [Summary("Search for song lyrics")]
        public async Task LyricsCommand([Remainder]string search)
        {

            var dom = await _web.GetDom($"{LyricsSearch}{Uri.EscapeUriString(search)}");
            var table = dom.GetElementsByClassName("panel")
                .FirstOrDefault(element => element.TextContent.Contains("Song results"))
                .GetElementsByClassName("table")
                .FirstOrDefault()?
                .FirstElementChild;

            if (table == null)
            {

                await ReplyAsync($"There are no results for **{search}**!");
                return;

            }

            var results = table.GetElementsByClassName("text-left")
                //.Skip(1)
                .Select(element =>
                {

                    try
                    {
                        element.RemoveChild(element.GetElementsByTagName("small").FirstOrDefault());
                    }
                    catch { }

                    return element;

                });

            var resultString = results.Select(element => element.TextContent.Trim());
            var resultLinks = results.Select(element => element.GetElementsByTagName("a").FirstOrDefault().GetAttribute("href"));

            var organizedResults = $"```\n" +
                $"Here are the top {results.Count()} results!\n\n" +
                $"{string.Join("\n", resultString)}\n\n" +
                $"Hit a number to see the result!```";

            await ReplyAsync(organizedResults);

            var response = await WaitForMessage(Context.User, Context.Channel, TimeSpan.FromSeconds(60));

            if (int.TryParse(response.Content, out var selection))
            {

                var link = resultLinks.ElementAtOrDefault(selection - 1);

                if (link == null)
                    return;

                var dmchannel = await Context.User.GetOrCreateDMChannelAsync();
                dom = await _web.GetDom(link);
                var mainDom = dom.GetElementsByClassName("col-xs-12 col-lg-8 text-center").FirstOrDefault();

                var lyricElement = mainDom.GetElementsByTagName("div")
                    .FirstOrDefault(element => element.ClassName == null || element.ClassName == string.Empty);

                var lyrics = Regex.Replace(lyricElement.InnerHtml, HtmlTagPattern, string.Empty)
                    .Replace("<br>", string.Empty)
                    //ultimate kek, pls forgive
                    .Replace("<!-- Usage of azlyrics.com content by any third-party lyrics provider is prohibited by our licensing agreement. Sorry about that. -->", string.Empty)
                    .Trim();
                //.Substring(0, 1500);
                lyrics = Regex.Replace(lyrics, HtmlTagPattern, string.Empty);

                var songName = mainDom.Children.FirstOrDefault(element => element.TagName == "B")?.TextContent;
                var artist = mainDom.GetElementsByClassName("lyricsh").FirstOrDefault()?.TextContent.Replace("Lyrics", string.Empty).Trim();
                var feat = mainDom.GetElementsByClassName("feat").FirstOrDefault()?.TextContent.Trim();

                try
                {

                    if (lyrics.Length > 1900)
                    {

                        var lyricsParts = new List<string>();

                        while (lyrics.Length > 0)
                        {

                            int index;

                            if (lyrics.Length > 1900)
                                index = 1900;
                            else
                                index = lyrics.Length;

                            var str = lyrics.Substring(0, index);
                            index = str.LastIndexOf("\n\n");

                            if (index == -1)
                                index = lyrics.Length;

                            lyricsParts.Add(lyrics.Substring(0, index).Trim());

                            lyrics = lyrics.Substring(index, Math.Abs(index - lyrics.Length)).Trim();

                        }

                        string print = $"```fix\n" +
                            $"{songName}\n" +
                            $"by {artist} ";

                        if (!string.IsNullOrEmpty(feat))
                            print += $"feat. {feat}";

                        print += $"\n\n{lyricsParts[0]}```";

                        await dmchannel.SendMessageAsync(print);
                        await ReplyAsync("Sent lyrics to DM's!"); //i put this here and rather late to check if DM could be sent

                        foreach (var lyric in lyricsParts.Skip(1))
                            await dmchannel.SendMessageAsync($"```fix\n{lyric}```");

                    }
                    else
                    {

                        string print = $"```fix\n" +
                            $"{songName}\n" +
                            $"by {artist} ";

                        if (!string.IsNullOrEmpty(feat))
                            print += $"feat. {feat}";

                        print += $"\n\n{lyrics}```";

                        await dmchannel.SendMessageAsync(print);
                        await ReplyAsync("Sent lyrics to DM's!");

                    }

                }
                catch (HttpException)
                {

                    await ReplyAsync("Please open your DM's because some lyrics may be too spammy to post in guild chat!");

                }

            }

        }

    }
}
