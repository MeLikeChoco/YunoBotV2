using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YunoBotV2.Configuration;
using YunoBotV2.Objects.Deserializers;
using YunoBotV2.Services;
using YunoBotV2.Services.WebServices;

namespace YunoBotV2.Commands
{
    public class Osu : CustomModuleBase
    {

        private Web _service;

        public Osu(Web serviceParams)
        {

            _service = serviceParams;

        }

        [Command("osur")]
        [Summary("Returns a random beatmap")]
        public async Task RandomOsuMapCommand(string mode = "4")
        {

            if (!(int.TryParse(mode, out int intMode)))
            {
                await DefaultErrorMessage();
                return;
            }

            EmbedBuilder body;

            using (Context.Channel.EnterTypingState())
            {                

                var randomYear = Rand.Next(2012, DateTime.Now.Year);
                var randomMonth = Rand.Next(1, 13);
                var randomDay = Rand.Next(1, 28); //account for feburary also too lazy to check if date is valid for that month
                var sqlDate = $"{randomYear}-{randomMonth}-{randomDay} 00:00:00";

                string key = Config.Osu;

                var baseUrl = $"https://osu.ppy.sh/api/get_beatmaps?&k={key}";
                var url = intMode == 4 ? $"{baseUrl}&since={sqlDate}" : $"{baseUrl}&since={sqlDate}&mode={intMode}";

                JArray beatmaps = await _service.GetJArrayContent(url);
                //get a random beatmapset id from a list of beatmaps
                string beatmapsetId = beatmaps[Rand.Next(0, beatmaps.Count)].Value<string>("beatmapset_id");

                url = $"{baseUrl}&s={beatmapsetId}";
                List<OsuBeatmapSet> mapset = await _service.GetDeserializedContent<List<OsuBeatmapSet>>(url);
                mapset = mapset.OrderBy(map => map.DifficultyRating).ToList();

                var author = new EmbedAuthorBuilder()
                    .WithName(mapset.First().Title)
                    .WithUrl($"https://osu.ppy.sh/s/{mapset.First().BeatmapSetId}");

                var footer = new EmbedFooterBuilder()
                    .WithText("Click the circles ♫ | " + mapset.First().ApprovedDate)
                    .WithIconUrl("http://vignette3.wikia.nocookie.net/osugame/images/c/c9/Logo.png/revision/latest?cb=20151219073209");
                    //.WithIconUrl("http://orig09.deviantart.net/ac11/f/2014/305/f/1/osu____spinner_circle_1_by_yunowhoitis-d84ni6l.png")

                body = new EmbedBuilder()
                {

                    Author = author,
                    Color = new Color(255, 105, 180),
                    Description = $"Song by: {mapset.First().Artist}\nCreated by: {mapset.First().Creator}",
                    ImageUrl = $"https://b.ppy.sh/thumb/{mapset.First().BeatmapSetId}l.jpg",
                    Footer = footer

                };

                body.AddInlineField("Genre", GetGenre(int.Parse(mapset.First().GenreId)));
                body.AddInlineField("Language", GetLanguage(int.Parse(mapset.First().LanguageId)));

                body.AddField("Plays", GetPlays(mapset));
                body.AddField("Favourited", $"{mapset.First().FavouriteCount} times");

                body.AddInlineField("Version", GetVersions(mapset));
                body.AddInlineField("Mode", GetMode(mapset));
                body.AddInlineField("Stars", GetDifficultyStars(mapset));

                body.AddField("Download", $"https://osu.ppy.sh/d/{mapset.First().BeatmapSetId}n");

            }

            await ReplyAsync("", embed: body);

        }

        [Command("osuu")]
        [Summary("Returns osu info on a user")]
        public async Task OsuUserCommand([Remainder]string user)
        {

            var url = $"https://osu.ppy.sh/api/get_user?k={Config.Osu}&u={Uri.EscapeUriString(user)}";
            EmbedBuilder body;

            using (Context.Channel.EnterTypingState())
            {

                JToken token = await _service.GetFirstJArrayContent(url);

                if (token == null)
                {
                    await ReplyAsync("Either service is down or user does not exist.");
                    return;
                }

                OsuUser osuUser = token.ToObject<OsuUser>();

                var author = new EmbedAuthorBuilder()
                    .WithName(osuUser.Username)
                    .WithUrl($"https://osu.ppy.sh/u/{Uri.EscapeUriString(osuUser.Username)}");

                var footer = new EmbedFooterBuilder()
                    .WithIconUrl("http://vignette3.wikia.nocookie.net/osugame/images/c/c9/Logo.png/revision/latest?cb=20151219073209")
                    .WithText("Click the circles ♫ | " + osuUser.Country);
                    //.WithIconUrl("http://orig09.deviantart.net/ac11/f/2014/305/f/1/osu____spinner_circle_1_by_yunowhoitis-d84ni6l.png")

                body = new EmbedBuilder()
                {

                    Author = author,
                    Color = new Color(255, 105, 180),
                    Description = "Global Rank: " + osuUser.PPRank +
                    "\nCountry Rank: " + osuUser.PPCountryRank +
                    "\nLevel: " + FormatLevel(osuUser.Level),
                    ImageUrl = $"https://a.ppy.sh/{osuUser.Id}",
                    Footer = footer

                };

                body.AddInlineField("PP", osuUser.PPRaw);
                body.AddInlineField("Accuracy", double.Parse(osuUser.Accuracy).ToString("0.##") + "%");
                body.AddInlineField("Total Amount of Plays", osuUser.PlayCount);

                body.AddInlineField("Total SS Ranks", osuUser.CountSS);
                body.AddInlineField("Total S Ranks", osuUser.CountS);
                body.AddInlineField("Total A Ranks", osuUser.CountA);

            }

            await ReplyAsync("", embed: body);

        }

        private string GetMode(List<OsuBeatmapSet> mapset)
        {

            string modes = "";

            foreach (OsuBeatmapSet o in mapset)
            {

                modes += ConvertNumberToMode(o.Mode) + "\n";

            }

            return modes;

        }

        private string ConvertNumberToMode(int mode)
        {

            switch (mode)
            {

                case 0:
                    return "osu!";
                case 1:
                    return "Taiko";
                case 2:
                    return "Catch the Beat";
                case 3:
                    return "osu!mania";
                default:
                    return "I DON'T KNOW WHAT MODE, YOU'RE FUCKED";

            }

        }

        private string GetDifficultyStars(List<OsuBeatmapSet> mapset)
        {

            string difficulties = "";

            foreach (OsuBeatmapSet o in mapset)
            {

                difficulties += string.Format("{0:0.##}", o.DifficultyRating) + " ★\n";

            }

            return difficulties;

        }

        private string GetVersions(List<OsuBeatmapSet> mapset)
        {

            string versions = "";

            foreach (OsuBeatmapSet o in mapset)
            {

                versions += o.Version + "\n";

            }

            return versions;

        }

        private string GetPlays(List<OsuBeatmapSet> mapset)
        {

            int totalPlays = 0;

            foreach (OsuBeatmapSet o in mapset)
            {

                totalPlays += int.Parse(o.PlayCount);

            }

            return totalPlays.ToString();

        }

        private string GetLanguage(int languageId)
        {

            // 0 = any, 1 = other, 2 = english, 3 = japanese, 4 = chinese, 5 = instrumental, 6 = korean, 7 = french, 8 = german, 9 = swedish, 10 = spanish, 11 = italian

            switch (languageId)
            {

                case 0:
                    return "Any (Added in myself: I don't even know how it could be any, probably bork music)";
                case 1:
                    return "Other";
                case 2:
                    return "English";
                case 3:
                    return "Japanese";
                case 4:
                    return "Chinese";
                case 5:
                    return "Instrumental";
                case 6:
                    return "Korean";
                case 7:
                    return "French";
                case 8:
                    return "German";
                case 9:
                    return "Swedish";
                case 10:
                    return "Spanish";
                case 11:
                    return "Italian";
                default:
                    return "Nothing was found, safe to say that it's probably alien";

            }

        }

        private string GetGenre(int genreId)
        {

            // 0 = any, 1 = unspecified, 2 = video game, 3 = anime, 4 = rock, 5 = pop, 6 = other, 7 = novelty, 9 = hip hop, 10 = electronic (note that there's no 8)

            switch (genreId)
            {

                case 0:
                    return "Any";
                case 1:
                    return "Unspecified";
                case 2:
                    return "Video Game";
                case 3:
                    return "Anime";
                case 4:
                    return "Rock";
                case 5:
                    return "Pop";
                case 6:
                    return "Other";
                case 7:
                    return "Novelty";
                case 9:
                    return "Hip-Hop";
                case 10:
                    return "Electronic";
                default:
                    return "SOME FUCKERY";

            }

        }

        private string FormatLevel(string userLevel)
        {

            if (double.TryParse(userLevel, out var level))
            {

                //even though there will be a 99.9999999% chance that levels wont be negative, ill use truncate
                var dec = (level - Math.Truncate(level)) * 100;
                var str = $"{Math.Truncate(level)} ({dec.ToString("#.##")}%)";

                return str;

            }
            else
                return null;

        }

    }
}
