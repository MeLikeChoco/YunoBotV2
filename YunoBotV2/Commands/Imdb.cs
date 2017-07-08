using Discord;
using Discord.Commands;
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
    [Group("imdb")]
    public class Imdb : CustomModuleBase
    {

        public Web _web { get; set; }        

        [Command]
        [Summary("Searches the imdb database")]
        public async Task MovieSearchCommand([Remainder]string search = "")
        {

            var url = $"http://www.omdbapi.com/?s={Uri.EscapeUriString(search)}&plot=full";
            JArray results;

            using (Context.Channel.EnterTypingState())
            {

                JObject o = await _web.GetJObjectContent(url);

                if(o == null)
                {
                    await DefaultErrorMessage();
                    return;
                }

                results = o.Value<JArray>("Search");

                if(results.Count == 1)
                {

                    await MovieCommand(results.First.Value<string>());
                    return;

                }

                var organizedResults = new StringBuilder($"```Showing {results.Count}/10\n\n");

                //omdb only returns 10 max, need to use page numbers to show more
                for (int i = 0; i < 10; i++)
                {

                    var token = results[i];

                    organizedResults.AppendLine($"{i + 1}: {token.Value<string>("Title")}");

                }

                organizedResults.Append("\nHit a number to see the corresponding result!```");

                await ReplyAsync(organizedResults.ToString());

            }

            var message = await WaitForMessage(Context.User, Context.Channel, TimeSpan.FromSeconds(60));

            if (int.TryParse(message.Content, out var selection) && selection > 0 && selection <= results.Count)
                await MovieCommand(results[selection - 1].Value<string>());

        }

        private async Task MovieCommand(string id)
        {

            if (string.IsNullOrEmpty(id))
            {
                await SearchErrorMessage();
                return;
            }

            var url = $"http://www.omdbapi.com/?i={id}&plot=full&r=json";

            using (Context.Channel.EnterTypingState())
            {

                ImdbMovie result = await _web.GetDeserializedContent<ImdbMovie>(url);

                if (string.IsNullOrEmpty(result.Title))
                {
                    await DefaultErrorMessage();
                    return;
                }

                var author = new EmbedAuthorBuilder()
                    .WithName(result.Title)
                    .WithUrl($"http://www.imdb.com/title/{result.ImdbId}");

                var footer = new EmbedFooterBuilder()
                    .WithText($"Uses OMDb API | {char.ToUpper(result.Type[0]) + result.Type.Substring(1)}")
                    .WithIconUrl("http://icons.iconarchive.com/icons/danleech/simple/256/imdb-icon.png");

                var countryYearRatedRuntime = $"**Country:** {result.Country}\n**Release Date:** {result.Released}\n**Rated:** {result.Rated}\n**Length:** {result.Runtime}";

                var body = new EmbedBuilder()
                {

                    Author = author,
                    Color = new Color(230, 185, 30),
                    Description = countryYearRatedRuntime,
                    ThumbnailUrl = result.Poster,
                    Footer = footer

                };

                body.AddInlineField("Director(s)", result.Director);
                body.AddInlineField("Writer(s)", result.Writer);
                body.AddInlineField("Actor(s)", result.Actors);
                body.AddField("Plot", result.Plot.Length > 1000 ? result.Plot.Substring(0, 1000) + $"...[Read More](http://www.imdb.com/title/{id})" : result.Plot);
                body.AddInlineField("Genre(s)", result.Genre);
                body.AddInlineField("Language(s)", result.Language);
                body.AddInlineField("Award(s)", result.Awards);
                body.AddField("Ratings", $"IMDb Rating: {result.ImdbRating}/10\nMetascore: {result.Metascore}/100");

                await ReplyAsync("", embed: body);

            }

        }

    }
}
