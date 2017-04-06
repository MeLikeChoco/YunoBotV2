using Discord;
using Discord.Commands;
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
    [Group("imdb")]
    public class Imdb : CustomModuleBase
    {

        private Web _service;

        public Imdb(Web serviceParams)
        {

            _service = serviceParams;

        }

        [Command("search", RunMode = RunMode.Async)]
        [Summary("Searches the imdb database")]
        public async Task MovieSearchCommand([Remainder]string search = "")
        {

            if (string.IsNullOrEmpty(search))
            {
                await SearchErrorMessage();
                return;
            }

            var url = $"http://www.omdbapi.com/?s={Uri.EscapeUriString(search)}&plot=full";
            var searchResults = new Dictionary<int, string>(20);

            using (Context.Channel.EnterTypingState())
            {

                JObject o = await _service.GetJObjectContent(url);

                if(o == null)
                {
                    await DefaultErrorMessage();
                    return;
                }

                var results = o.Value<JArray>("Search");
                var organizedResults = new StringBuilder($"```Showing {results.Count}/10\n\n");
                var counter = 1;

                //omdb only returns 10 max, need to use page numbers to show more
                foreach(JToken token in results)
                {
                    
                    organizedResults.AppendLine($"{counter}: {token.Value<string>("Title")}");
                    searchResults.Add(counter, token.Value<string>("imdbID"));
                    counter++;

                }

                organizedResults.Append("\nHit a number to see the corresponding result!```");

                await ReplyAsync(organizedResults.ToString());

            }

            IUserMessage message = await WaitForMessage(Context.User, Context.Channel, TimeSpan.FromSeconds(60));

            if (int.TryParse(message.Content, out int movie))
            {
                if (searchResults.TryGetValue(movie, out string id))
                    await MovieCommand(id);
            }

        }

        [Command]
        [Summary("Return a imdb result based on id")]
        public async Task MovieCommand(string id = "")
        {

            if (string.IsNullOrEmpty(id))
            {
                await SearchErrorMessage();
                return;
            }

            var url = $"http://www.omdbapi.com/?i={id}&plot=full&r=json";

            using (Context.Channel.EnterTypingState())
            {

                ImdbMovie result = await _service.GetDeserializedContent(url, typeof(ImdbMovie));

                if (string.IsNullOrEmpty(result.Title))
                {
                    await DefaultErrorMessage();
                    return;
                }

                var authorBuilder = new EmbedAuthorBuilder()
                {

                    Name = "IMDb",
                    Url = "http://www.imdb.com/",
                    IconUrl = "http://icons.iconarchive.com/icons/danleech/simple/256/imdb-icon.png"

                };

                var footerBuilder = new EmbedFooterBuilder()
                {

                    Text = $"Uses OMDb API | {char.ToUpper(result.Type[0]) + result.Type.Substring(1)}",
                    IconUrl = "https://cdn0.iconfinder.com/data/icons/cosmo-multimedia/40/movie_4-512.png"

                };

                var countryYearRatedRuntime = $"**Country:** {result.Country}\n**Release Date:** {result.Released}\n**Rated:** {result.Rated}\n**Length:** {result.Runtime}";

                var eBuilder = new EmbedBuilder()
                {

                    Author = authorBuilder,
                    Color = new Color(230, 185, 30),
                    Title = result.Title,
                    Description = countryYearRatedRuntime,
                    Url = $"http://www.imdb.com/title/{result.imdbID}",
                    ThumbnailUrl = result.Poster,
                    Footer = footerBuilder

                };

                eBuilder.AddField(x =>
                {
                    x.Name = "Director(s)";
                    x.Value = result.Director;
                });

                eBuilder.AddField(x =>
                {
                    x.Name = "Writer(s)";
                    x.Value = result.Writer;
                });

                eBuilder.AddField(x =>
                {
                    x.Name = "Actor(s)";
                    x.Value = result.Actors;
                });

                eBuilder.AddField(x =>
                {
                    x.Name = "Plot";
                    x.Value = result.Plot.Length > 1000 ? result.Plot.Substring(0, 1000) + $"...[Read More](http://www.imdb.com/title/{id})" : result.Plot;
                });

                eBuilder.AddField(x =>
                {
                    x.Name = "Genre(s)";
                    x.Value = result.Genre;
                });

                eBuilder.AddField(x =>
                {
                    x.Name = "Language(s)";
                    x.Value = result.Language;
                });

                eBuilder.AddField(x =>
                {
                    x.Name = "Award(s)";
                    x.Value = result.Awards;
                });

                eBuilder.AddField(x =>
                {
                    x.Name = "Ratings";
                    x.Value = $"IMDb Rating: {result.imdbRating}/10\nMetascore: {result.Metascore}/100";
                });

                await ReplyAsync("", embed: eBuilder);

            }

        }

    }
}
