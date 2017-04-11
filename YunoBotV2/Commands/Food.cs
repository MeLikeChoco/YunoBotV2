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
using YunoBotV2.Deserializers;
using Discord;

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

                JArray hits = await _service.GetDeserializedContent(url, typeof(JArray), "hits");

                if(hits.Count == 0)
                {
                    await NoResultsReturnedErrorMessage();
                    return;
                }

                Recipe recipe = hits.ElementAt(new Random().Next(0, hits.Count))["recipe"].ToObject<Recipe>();

                var authorBuilder = new EmbedAuthorBuilder()
                {

                    Name = "Edamam Recipes",
                    IconUrl = "http://cdn.appstorm.net/ipad.appstorm.net/authors/jessotoole/Edamam-icon.jpg",
                    Url = "https://www.edamam.com/"

                };

                var footerBuilder = new EmbedFooterBuilder()
                {

                    Text = "Brought to you by Edamam Recipes | Please support them, they allow 5000 free api calls!",
                    IconUrl = "https://pbs.twimg.com/profile_images/3414170675/d90fcd92b9a6f13b9f894fd412620c7e_400x400.png"

                };

                var dietLabels = string.Join(", ", recipe.dietLabels);
                var healthLabels = string.Join(", ", recipe.healthLabels);

                var eBuilder = new EmbedBuilder()
                {

                    Author = authorBuilder,
                    Color = new Color(122, 214, 25),
                    Title = recipe.label,
                    Url = recipe.url,
                    ThumbnailUrl = recipe.image,
                    Description = $"**Servings:** {(int)double.Parse(recipe.yield)}\n**Calories:** {(int)double.Parse(recipe.calories)}\n**Diet Labels:** {dietLabels}\n**Health Labels:** {healthLabels}",
                    Footer = footerBuilder

                };

                string ingredients = string.Empty;
                recipe.ingredientLines.ForEach(i => ingredients += $"{i}\n");

                eBuilder.AddField(x =>
                {

                    x.Name = "Ingredients";
                    x.Value = ingredients;
                    x.IsInline = false;

                });

                await ReplyAsync("", embed: eBuilder);

            }

        }

    }
}
