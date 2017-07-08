using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunoBotV2.Objects.Deserializers
{
    public class General
    {
        public int goldEarned { get; set; }
        public double neutralMinionsKilledEnemyJungle { get; set; }
        public double neutralMinionsKilledTeamJungle { get; set; }
        public double minionsKilled { get; set; }
        public double largestKillingSpree { get; set; }
        public int totalHeal { get; set; }
        public int totalDamageTaken { get; set; }
        public int totalDamageDealtToChampions { get; set; }
        public double assists { get; set; }
        public double deaths { get; set; }
        public double kills { get; set; }
        public double experience { get; set; }
        public double banRate { get; set; }
        public double playPercent { get; set; }
        public double winPercent { get; set; }
    }

    public class ChampionGGStats
    {
        public string key { get; set; }
        public string role { get; set; }
        public string title { get; set; }
        public General general { get; set; }
    }
}
