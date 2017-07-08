using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using YunoBotV2.Services.WebServices;
using Newtonsoft.Json.Linq;
using YunoBotV2.Configuration;
using Newtonsoft.Json;
using YunoBotV2.Objects.Deserializers;
using Discord;
using YunoBotV2.Services;

namespace YunoBotV2.Commands
{
    public class Food : CustomModuleBase
    {

        private Web _service;

        public Food(Web serviceParams)
        {

            _service = serviceParams;

        }

        [Command("recipe")]
        [Summary("Get a random recipe because you suck at cooking")]
        public async Task RecipeCommand([Remainder]string search = "")
        {

            if (string.IsNullOrEmpty(search))
            {
                await SearchErrorMessage();
                return;
            }

            var url = $"https://api.edamam.com/search?app_id={Config.EdamamId}&app_key={Config.EdamamKey}&q={Uri.EscapeUriString(search)}";

            using (Context.Channel.EnterTypingState())
            {

                var hits = await _service.GetDeserializedContent<JArray>(url, "hits");

                if (hits.Count == 0)
                {
                    await NoResultsReturnedErrorMessage();
                    return;
                }

                Recipe recipe = hits.ElementAt(Rand.Next(0, hits.Count))["recipe"].ToObject<Recipe>();

                var author = new EmbedAuthorBuilder()
                    .WithName(recipe.Label)
                    .WithUrl(recipe.Url);

                var footer = new EmbedFooterBuilder()
                    .WithText("Brought to you by Edamam Recipes | Please support them, they allow 5000 free api calls!")
                    .WithIconUrl("https://pbs.twimg.com/profile_images/3414170675/d90fcd92b9a6f13b9f894fd412620c7e_400x400.png");

                var dietLabels = string.Join(", ", recipe.DietLabels);
                var healthLabels = string.Join(", ", recipe.HealthLabels);

                var body = new EmbedBuilder()
                {

                    Author = author,
                    Color = new Color(122, 214, 25),
                    ThumbnailUrl = recipe.Image,
                    Description = $"**Servings:** {(int)double.Parse(recipe.Yield)}\n**Calories:** {(int)double.Parse(recipe.Calories)}\n**Diet Labels:** {dietLabels}\n**Health Labels:** {healthLabels}",
                    Footer = footer

                };

                body.AddField("Ingredients", recipe.Ingredients.Aggregate((str, next) => $"{str}\n{next}"));

                await ReplyAsync("", embed: body);

            }

        }

    }
}
