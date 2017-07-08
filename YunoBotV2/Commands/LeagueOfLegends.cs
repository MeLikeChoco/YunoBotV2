using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord;
using YunoBotV2.Services.WebServices;
using System.Text.RegularExpressions;
using YunoBotV2.Configuration;
using Newtonsoft.Json.Linq;
using YunoBotV2.Objects.Deserializers;

namespace YunoBotV2.Commands
{
    [Group("lol")]
    public class LeagueOfLegends : CustomModuleBase
    {

        private Web _service;

        public LeagueOfLegends(Web serviceParams)
        {

            _service = serviceParams;

        }

        [Command]
        [Summary("Return a champion's stats")]
        public async Task ChampionCommand([Remainder]string champion)
        {

            var pattern = new Regex("\\s+");
            champion = pattern.Replace(champion, "").ToLower();
            champion = champion.Replace("'", "");

            var url = $"http://api.champion.gg/champion/{Uri.EscapeUriString(champion)}?api_key={Config.ChampionGG}";

            JArray array = await _service.GetJArrayContent(url);

            if(array == null || array.Count == 0)
            {
                await NoResultsReturnedErrorMessage();
                return;
            }

            var organizedResults = $"```Here are the roles for {array.First()["key"]}\n\n";
            var counter = 1;

            foreach(JToken token in array)
            {
                organizedResults += $"{counter}. {token["role"]}\n";
                counter++;
            }

            organizedResults += "\nHit a number to see information on that role!```";

            await ReplyAsync(organizedResults);

            IMessage message = await WaitForMessage(Context.User, Context.Channel, TimeSpan.FromSeconds(60));

            ChampionGG result;

            if (int.TryParse(message.Content, out int selection))
            {
                if (selection > array.Count)
                    return;

                result = array[selection - 1].ToObject<ChampionGG>();
            }
            else
                return;

            url = $"http://api.champion.gg/stats/champs/{result.key}?api_key={Config.ChampionGG}";

            array = await _service.GetJArrayContent(url);
            var stats = array.Where(token => token["role"].ToString().Equals(result.role)).First().ToObject<ChampionGGStats>(); //get stats
            var generalStats = stats.general;

            var authorBuilder = new EmbedAuthorBuilder()
                .WithName("League of Legends")
                .WithUrl("http://www.leagueoflegends.com/")
                .WithIconUrl("http://orig01.deviantart.net/907e/f/2015/025/5/5/league_of_legends__connected_fates__episode_2_by_lol_connectedfates-d8fgb24.jpg");

            var footerBuilder = new EmbedFooterBuilder()
                .WithText("Brought to you by ChampionGG");

            var eBuilder = new EmbedBuilder()
            {

                Author = authorBuilder,
                Color = new Color(195, 167, 44),
                Url = $"http://champion.gg/champion/{result.key}/",
                ThumbnailUrl = $"http://ddragon.leagueoflegends.com/cdn/6.24.1/img/champion/{stats.key}.png",
                Title = stats.title,
                Description = $"**Role:** {result.role}\n**Play Rate:** {generalStats.playPercent}%\n**Win Rate:** {generalStats.winPercent}%\n**Ban Rate:** {generalStats.banRate}%",
                //Description = $"**Role:** {result.role}\n**Average Gold Earned:** {generalStats.goldEarned}\n" +
                //$"**Average Jungle Minions Killed:** {generalStats.neutralMinionsKilledEnemyJungle + generalStats.neutralMinionsKilledTeamJungle}\n" +
                //$"**Average Minions Killed:** {generalStats.minionsKilled}\n**Average Largest Killing Spree:** {generalStats.largestKillingSpree}\n" +
                //$"**Average Total Heal:** {generalStats.totalHeal}\n**Average Damage Taken:** {generalStats.totalDamageTaken}\n" +
                //$"**Average Damage Dealt To Champions:** {generalStats.totalDamageDealtToChampions}\n**Average Kills:** {generalStats.kills}\n" +
                //$"**Average Deaths:** {generalStats.deaths}\n**Average Assists:** {generalStats.assists}\n**Ban Rate:** {generalStats.banRate}%\n" +
                //$"**Play Rate:** {generalStats.playPercent}%\n**Win Rate:** {generalStats.winPercent}%",
                Footer = footerBuilder,

            };

            eBuilder.AddInlineField("Most Used Starting Items", string.Join(", ", result.FirstItems.mostGames.items.Select(item => item.name)));
            eBuilder.AddInlineField("Highest Win Rate Starting Items", string.Join(", ", result.FirstItems.highestWinPercent.items.Select(item => item.name)));

            eBuilder.AddInlineField("Most Used Final Build", string.Join(", ", result.FinalBuild.mostGames.items.Select(item => item.name)));
            eBuilder.AddInlineField("Highest Win Rate Final Build", string.Join(", ", result.FinalBuild.highestWinPercent.items.Select(item => item.name)));

            eBuilder.AddField(x =>
            {
                x.Name = "Trinkets Used";
                x.Value = string.Join("\n", result.Trinkets.Select(trinket => $"{trinket.item.name}: Winrate >> {trinket.winPercent}%"));
            });

            eBuilder.AddInlineField("Most Used Summoner Spells", result.SummonerSpells.mostGames.summoner1.name + " / " + result.SummonerSpells.mostGames.summoner2.name);
            eBuilder.AddInlineField("Highest Win Rate Summoner Spells", result.SummonerSpells.highestWinPercent.summoner1.name + " / " + result.SummonerSpells.highestWinPercent.summoner2.name);

            eBuilder.AddField("Most Used Runes", string.Join("\n", result.Runes.mostGames.runes.Select(rune => $"{rune.name}: {rune.number}")));
            eBuilder.AddField("Highest Win Rate Runes", string.Join("\n", result.Runes.highestWinPercent.runes.Select(rune => $"{rune.name}: {rune.number}")));
            //eBuilder.AddInlineField("Most Used Runes", string.Join("\n", result.Runes.mostGames.runes.Select(rune => $"{rune.name}: {rune.number}")));
            //eBuilder.AddInlineField("Highest Win Rate Runes", string.Join("\n", result.Runes.highestWinPercent.runes.Select(rune => $"{rune.name}: {rune.number}")));

            eBuilder.AddInlineField("Most Used Masteries", string.Join("\n", result.Masteries.mostGames.masteries.Select(mastery => $"{mastery.tree}: {mastery.total}")));
            eBuilder.AddInlineField("Highest Win Rate Masteries", string.Join("\n", result.Masteries.highestWinPercent.masteries.Select(mastery => $"{mastery.tree}: {mastery.total}")));

            eBuilder.AddInlineField("Most Used Skill Order", string.Join(" ", result.Skills.mostGames.order));
            eBuilder.AddInlineField("Highest Win Rate Skill Order", string.Join(" ", result.Skills.highestWinPercent.order));

            await ReplyAsync("", embed: eBuilder);

        }

    }
}
