using Discord;
using Discord.Commands;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YunoBotV2.Services;
using YunoBotV2.Services.WebServices;

namespace YunoBotV2.Commands
{
    public class Pokemon : CustomModuleBase
    {

        private Web _service;

        public Pokemon(Web serviceParams)
            => _service = serviceParams;

        [Command("pokemon")]
        [Summary("Search for pokemon")]
        public async Task PokemonCommand([Remainder]string search = "")
        {

            if (string.IsNullOrEmpty(search))
            {
                await SearchErrorMessage();
                return;
            }

            var url = $"https://pokeapi.co/api/v2/pokemon-species/{Uri.EscapeUriString(search)}";
            var results = await _service.GetDeserializedContent<JArray>(url, "varieties");

            if (results == null || results.Count == 0)
            {
                await NoResultsReturnedErrorMessage();
                return;
            }

            string organizedResults = "```Here are your results!\n\n"; //pretty sure i dont need stringbuilder, didnt really need it in the other cases either, but might as well ingrain basic "optimization" in me
            int counter = 1;
            var resultDict = new Dictionary<int, string>();

            foreach (var token in results)
            {
                organizedResults += $"{counter}. {token["pokemon"]["name"]}\n";
                resultDict.Add(counter, token["pokemon"]["url"].ToString());
            }

            organizedResults += organizedResults += "\nHit a number to see that pokemon!```";

            IMessage message = await WaitForMessage(Context.User, Context.Channel, TimeSpan.FromSeconds(60));

            if (!int.TryParse(message.Content, out int key))
                return;

            if (resultDict.TryGetValue(key, out url))
            {
                if (Cache.Pokemon.TryGetValue(url, out EmbedBuilder eBuilder))
                {
                    await ReplyAsync("", embed: eBuilder);
                }
                else
                {

                    //var pokemon = await _service.GetDeserializedContent

                    var authorBuilder = new EmbedAuthorBuilder
                    {

                        Name = "Pokemon",
                        Url = "http://www.pokemon.com",
                        IconUrl = "https://cdn4.iconfinder.com/data/icons/longico/224/longico-23-512.png",

                    };

                    var footerBuilder = new EmbedFooterBuilder
                    {

                        Text = $"Brought to you by PokeAPI | {Context.Client.GetApplicationInfoAsync().Result.Owner.Username}",
                        IconUrl = "https://pbs.twimg.com/profile_images/378800000751640307/4dbd22afba75e8cedf0c680cd38a7876_400x400.png",

                    };

                    eBuilder = new EmbedBuilder
                    {

                        Author = authorBuilder,
                        Color = new Color(236, 71, 63),
                        //ThumbnailUrl

                    };

                }
            }

        }

    }
}
