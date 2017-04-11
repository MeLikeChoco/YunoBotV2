using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunoBotV2.Deserializers
{

    //<------------ items/Final Build ------------------->
    public class Items
    {
        public ItemsStats mostGames { get; set; }
        public ItemsStats highestWinPercent { get; set; }
    }

    public class ItemsStats
    {
        public List<Item> items { get; set; }
        public double winPercent { get; set; }
        public int games { get; set; }
    }
    //<------------------------------------------->
    //<------------ firstItems/FirstItems ---------------->
    public class FirstItems
    {
        public ItemsStats mostGames { get; set; }
        public ItemsStats highestWinPercent { get; set; }
    }
    //<-------------------------------------------->
    //<----------- trinkets ----------------------->
    public class Trinket
    {
        public int games { get; set; }
        public double winPercent { get; set; }
        public Item item { get; set; }
    }

    public class Item
    {
        public int id { get; set; }
        public string name { get; set; }
    }
    //<---------------------------------------------->
    //<----------- summoners/SummonerSpells ----------->
    public class Summoners
    {
        public SummonerSpellStats mostGames { get; set; }
        public SummonerSpellStats highestWinPercent { get; set; }
    }

    public class SummonerSpellStats
    {
        public Summoner summoner1 { get; set; }
        public Summoner summoner2 { get; set; }
        public double winPercent { get; set; }
        public int games { get; set; }
    }

    public class Summoner
    {
        public string name { get; set; }
    }
    //<----------------------------------------------->
    //<----------------- Runes -------------------->
    public class Runes
    {
        public RunesStats mostGames { get; set; }
        public RunesStats highestWinPercent { get; set; }
    }

    public class RunesStats
    {
        public List<Rune> runes { get; set; }
        public double winPercent { get; set; }
        public int games { get; set; }
    }

    public class Rune
    {
        public int id { get; set; }
        public int number { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }
    //<------------------------------------------->
    //<-------------- DmgComposition ------------>
    public class DmgComposition
    {
        public double trueDmg { get; set; }
        public double magicDmg { get; set; }
        public double physicalDmg { get; set; }
    }
    //<------------------------------------------->
    //<-------------- Skills ------------------>
    public class Skills
    {
        public List<SkillInfo> skillInfo { get; set; }
        public SkillsStats mostGames { get; set; }
        public SkillsStats highestWinPercent { get; set; }
    }

    public class SkillInfo
    {
        public string name { get; set; }
        public string key { get; set; }
    }

    public class SkillsStats
    {
        public List<string> order { get; set; }
        public double winPercent { get; set; }
        public int games { get; set; }
    }
    //<------------------------------------------>
    //<-------------- Masteries ---------------------->
    public class Masteries
    {
        public MasteriesStats mostGames { get; set; }
        public MasteriesStats highestWinPercent { get; set; }
    }

    public class MasteriesStats
    {
        public double winPercent { get; set; }
        public int games { get; set; }
        public List<Mastery> masteries { get; set; }
    }

    public class Mastery
    {
        public string tree { get; set; }
        public int total { get; set; }
    }
    //<-------------------------------------------->
    public class ChampionGG
    {
        public string key { get; set; }
        public string role { get; set; }
        [JsonProperty("items")]
        public Items FinalBuild { get; set; }
        [JsonProperty("firstItems")]
        public FirstItems FirstItems { get; set; }
        [JsonProperty("trinkets")]
        public List<Trinket> Trinkets { get; set; }
        [JsonProperty("summoners")]
        public Summoners SummonerSpells { get; set; }
        [JsonProperty("runes")]
        public Runes Runes { get; set; }
        [JsonProperty("patchWin")]
        public List<double> WinRateForLast6Patches { get; set; }
        [JsonProperty("patchPlay")]
        public List<double> PlayRateForLast6Patches { get; set; }
        [JsonProperty("dmgComposition")]
        public DmgComposition DmgComposition { get; set; }
        [JsonProperty("skills")]
        public Skills Skills { get; set; }
        [JsonProperty("masteries")]
        public Masteries Masteries { get; set; }
    }

}
