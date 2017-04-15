using Discord;
using Discord.Commands;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YunoBotV2.Configuration;
using YunoBotV2.Core;
using YunoBotV2.Deserializers;
using YunoBotV2.Services.WebServices;

namespace YunoBotV2.Commands
{
    public class Weaboo : CustomModuleBase
    {

        private Web _service;
        private const string BaseUrl = "https://anilist.co/api";

        public Weaboo(Web serviceParams)
        {
            _service = serviceParams;
        }

        [Command("anime")]
        [Summary("Searches anime")]
        public async Task AnimeSearchCommand([Remainder]string search)
        {

            var url = $"{BaseUrl}/anime/search/{Uri.EscapeUriString(search)}?access_token={Config.AnilistToken}";
            var resultDict = new Dictionary<int, JToken>(10);

            using (Context.Channel.EnterTypingState())
            {

                JArray results = await _service.GetJArrayContent(url);

                if (results == null)
                {

                    if (!await Authenticate())
                    {
                        await DefaultErrorMessage();
                        return;
                    }
                    else
                    {
                        url = url = $"{BaseUrl}/anime/search/{Uri.EscapeUriString(search)}?access_token={Config.AnilistToken}"; //to refresh the token used
                        results = await _service.GetJArrayContent(url);
                    }

                }

                if (results.Count == 0)
                {
                    await NoResultsReturnedErrorMessage();
                    return;
                }
                else if (results.Count > 11)
                {
                    await RefineSearchErrorMessage();
                    return;
                }
                else if (results.Count == 1)
                {
                    await DisplayAnimeManga(results.First());
                    return;
                }

                var organizedResults = new StringBuilder("```Here is what I found!\n\n");
                var counter = 1;

                foreach (JToken token in results)
                {

                    organizedResults.AppendLine($"{counter}. {token["title_romaji"]}");
                    resultDict.Add(counter, token);
                    counter++;

                }

                organizedResults.Append("Hit a number to see information on that anime!```");

                await ReplyAsync(organizedResults.ToString());

            }

            IMessage message = await WaitForMessage(Context.User, Context.Channel, TimeSpan.FromSeconds(60));

            if (int.TryParse(message.Content, out int choice))
            {
                if (resultDict.TryGetValue(choice, out JToken id))
                {

                    using (Context.Channel.EnterTypingState())
                    {
                        await DisplayAnimeManga(id);
                    }

                }
            }

        }
        [Command("manga")]
        [Summary("Searches anime")]
        public async Task MangaSearchCommand([Remainder]string search)
        {

            var url = $"{BaseUrl}/manga/search/{Uri.EscapeUriString(search)}?access_token={Config.AnilistToken}";
            var resultDict = new Dictionary<int, JToken>(10);

            using (Context.Channel.EnterTypingState())
            {

                JArray results = await _service.GetJArrayContent(url);

                if (results == null)
                {

                    if (!await Authenticate())
                    {
                        await DefaultErrorMessage();
                        return;
                    }
                    else
                    {
                        url = url = $"{BaseUrl}/manga/search/{Uri.EscapeUriString(search)}?access_token={Config.AnilistToken}"; //to refresh the token used
                        results = await _service.GetJArrayContent(url);
                    }

                }

                if (results.Count == 0)
                {
                    await NoResultsReturnedErrorMessage();
                    return;
                }
                else if (results.Count > 11)
                {
                    await RefineSearchErrorMessage();
                    return;
                }
                else if (results.Count == 1)
                {
                    await DisplayAnimeManga(results.First());
                    return;
                }

                var organizedResults = new StringBuilder("```Here is what I found!\n\n");
                var counter = 1;

                foreach (JToken token in results)
                {

                    organizedResults.AppendLine($"{counter}. {token["title_romaji"]}");
                    resultDict.Add(counter, token);
                    counter++;

                }

                organizedResults.Append("Hit a number to see information on that anime!```");

                await ReplyAsync(organizedResults.ToString());

            }

            IMessage message = await WaitForMessage(Context.User, Context.Channel, TimeSpan.FromSeconds(60));

            if (int.TryParse(message.Content, out int choice))
            {
                if (resultDict.TryGetValue(choice, out JToken id))
                {

                    using (Context.Channel.EnterTypingState())
                    {
                        await DisplayAnimeManga(id);
                    }

                }
            }

        }

        [Command("character")]
        [Summary("Get information on a anime/manga character")]
        public async Task CharacterCommand([Remainder]string search)
        {

            var url = $"{BaseUrl}/character/search/{Uri.EscapeUriString(search)}?access_token={Config.AnilistToken}";
            var resultDict = new Dictionary<int, JToken>();

            using (Context.Channel.EnterTypingState())
            {

                JArray results = await _service.GetJArrayContent(url);

                if (results == null)
                {

                    if (!await Authenticate())
                    {
                        await DefaultErrorMessage();
                        return;
                    }
                    else
                    {
                        url = url = $"{BaseUrl}/character/search/{Uri.EscapeUriString(search)}?access_token={Config.AnilistToken}"; //to refresh the token used
                        results = await _service.GetJArrayContent(url);
                    }

                }

                if (results.Count == 0)
                {
                    await NoResultsReturnedErrorMessage();
                    return;
                }
                else if (results.Count > 21) //the amount of characters that share the same name is astounding...
                {
                    await RefineSearchErrorMessage();
                    return;
                }
                else if (results.Count == 1)
                {
                    await DisplayCharacter(results.First());
                    return;
                }

                var organizedResults = new StringBuilder("```Here is what I found!\n\n");
                var counter = 1;

                foreach (JToken token in results)
                {

                    organizedResults.AppendLine($"{counter}. {token["name_first"]} {token["name_last"]}");
                    resultDict.Add(counter, token);
                    counter++;

                }

                organizedResults.Append("Hit a number to see information on that anime!```");

                await ReplyAsync(organizedResults.ToString());

            }

            IMessage message = await WaitForMessage(Context.User, Context.Channel, TimeSpan.FromSeconds(60));

            if (int.TryParse(message.Content, out int choice))
            {
                if (resultDict.TryGetValue(choice, out JToken id))
                {

                    using (Context.Channel.EnterTypingState())
                    {
                        await DisplayCharacter(id);
                    }

                }
            }

        }

        [Command("anilist")]
        [Summary("Get access token")]
        [RequireOwner]
        public Task AnilistCommand()
            => Task.Run(() => AltConsole.Print("Command", "Weaboo", $"Access Token: {Config.AnilistToken}"));

        public async Task DisplayCharacter(JToken token)
        {

            var character = token.ToObject<AnimeMangaCharacter>();

            var authorBuilder = new EmbedAuthorBuilder
            {

                Name = "Anilist",
                Url = "https://anilist.co",
                IconUrl = "https://pbs.twimg.com/profile_images/485448147607371776/a3ExdM_a.png",

            };

            var footerBuilder = new EmbedFooterBuilder
            {

                Text = $"Your waifu is shit | {Context.Client.GetApplicationInfoAsync().Result.Owner.Username}",
                IconUrl = "http://data.whicdn.com/images/247168735/superthumb.jpg",

            };

            var eBuilder = new EmbedBuilder
            {

                Author = authorBuilder,
                Color = new Color(255, 183, 197),
                Title = $"{character.name_first} {character.name_last}",
                Description = $"**Japanese Name:** {character.name_japanese ?? "N/A"}\n**Alternate Name:** {character.name_alt ?? "N/A"}",
                ThumbnailUrl = character.image_url_med,
                Url = $"https://anilist.co/character/{character.id}",
                Footer = footerBuilder,

            };

            eBuilder.AddField(x =>
            {
                x.Name = "Information";
                x.Value = IsOverLimit("character", character.id, character.info, out string info) ? info : info;
            });

            await ReplyAsync("", embed: eBuilder);

        }

        public async Task DisplayAnimeManga(JToken token)
        {

            var result = token.ToObject<AnimeManga>();
            string description;

            if (result.total_episodes == null)
            {

                //for manga
                description = $"**English Title:** {result.title_english}\n**Japanese Title:** {result.title_japanese}\n**Type:** {CapitalizeFirstLetter(result.type)}\n" +
                    $"**Volumes:** {result.total_volumes}\n**Chapters:** {result.total_chapters}\n**Maturity Rating:** {(result.adult ? "Mature" : "Safe")}\n" +
                    $"**Start Date:** {FormatDateTime(result.start_date_fuzzy)}\n**End Date:** {FormatDateTime(result.end_date_fuzzy)}\n**Status:** {CapitalizeFirstLetter(result.publishing_status)}";

            }
            else
            {

                //for anime
                description = $"**English Title:** {result.title_english}\n**Japanese Title:** {result.title_japanese}\n**Type:** {char.ToUpper(result.type.First()) + result.type.Substring(1)}\n" +
                    $"**Episodes:** {result.total_episodes}\n**Episode Duration:** {result.duration}\n**Maturity Rating:** {(result.adult ? "Mature" : "Safe")}\n" +
                    $"**Start Date:** {FormatDateTime(result.start_date_fuzzy)}\n**End Date:** {FormatDateTime(result.end_date_fuzzy)}\n**Status:** {CapitalizeFirstLetter(result.airing_status)}";

            }

            var authorBuilder = new EmbedAuthorBuilder
            {

                IconUrl = "https://anilist.co/img/logo_al.png",
                Name = "Anilist",
                Url = "https://anilist.co/",

            };

            var footerBuilder = new EmbedFooterBuilder
            {

                IconUrl = "http://img00.deviantart.net/e0d8/i/2011/241/f/6/my_computer_anime_icon_by_hikanepb-d3j5a2v.png",
                Text = "Brought to you by Anilist | Because screw MAL and XML",

            };

            var eBuilder = new EmbedBuilder
            {

                Author = authorBuilder,
                Color = new Color(255, 183, 197),
                Url = $"https://anilist.co/anime/{result.id}",
                ThumbnailUrl = result.image_url_med,
                ImageUrl = result.image_url_banner,
                Title = result.title_romaji,
                Description = description,
                Footer = footerBuilder,

            };

            eBuilder.AddField(x =>
            {
                x.Name = "Genres";
                x.Value = string.Join(" / ", result.genres);
            });

            if (IsOverLimit(result.series_type, result.id, result.description, out string shortened))
            {
                eBuilder.AddField(x =>
                {
                    x.Name = "Description";
                    x.Value = shortened;
                });
            }
            else
            {
                eBuilder.AddField(x =>
                {
                    x.Name = "Description";
                    x.Value = shortened; //returns same value if false
                });
            }

            await ReplyAsync("", embed: eBuilder);

        }

        private async Task<bool> Authenticate()
        {

            var url = $"{BaseUrl}/auth/access_token";
            var payload = new FormUrlEncodedContent(new[]
            {

                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", Config.AnilistId),
                new KeyValuePair<string, string>("client_secret", Config.AnilistSecret),

            });

            if (await _service.Post(url, payload, out string result))
            {
                Config.AnilistToken = JObject.Parse(result)["access_token"].ToString();
                return true;
            }
            else
            {
                return false;
            }

        }

        private bool IsOverLimit(string type, string id, string description, out string shortened)
        {

            var regex = new Regex(@"\s+[_]+");

            if (description.Length > 951)
            {
                shortened = WebUtility.HtmlDecode(description);
                shortened = shortened.Substring(0, 950) + $"...[Read More](https://anilist.co/{type}/{id})";
                shortened = shortened.Replace("<br><br>", "\n");
                shortened = shortened.Replace("<br>", "\n");
                shortened = regex.Replace(shortened, "\n__");
                shortened = shortened.Replace("  ", "\n");
                return true;
            }
            else
            {
                shortened = WebUtility.HtmlDecode(description);
                shortened = shortened.Replace("<br><br>", "\n");
                shortened = shortened.Replace("<br>", "\n");
                shortened = regex.Replace(shortened, "\n__");
                shortened = shortened.Replace("  ", "\n");
                return false;
            }

        }

        private string CapitalizeFirstLetter(string str)
        {
            return char.ToUpper(str.First()) + str.Substring(1);
        }

        private string FormatDateTime(string date)
        {
            if (date != null)
                return DateTime.ParseExact(date, "yyyyMMdd", null).ToString("MM/dd/yyyy");
            else
                return "N/A";
        }

    }
}
