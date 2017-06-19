using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunoBotV2.Deserializers
{
    public class OsuUser
    {
        [JsonProperty("user_id")]
        public string Id { get; set; }
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("count300")]
        public string Count300 { get; set; }
        [JsonProperty("count100")]
        public string Count100 { get; set; }
        [JsonProperty("count50")]
        public string Count50 { get; set; }
        [JsonProperty("playcount")]
        public string PlayCount { get; set; }
        [JsonProperty("ranked_score")]
        public string RankedScore { get; set; }
        [JsonProperty("total_score")]
        public string TotalScore { get; set; }
        [JsonProperty("pp_rank")]
        public string PPRank { get; set; }
        [JsonProperty("level")]
        public string Level { get; set; }
        [JsonProperty("pp_raw")]
        public string PPRaw { get; set; }
        [JsonProperty("accuracy")]
        public string Accuracy { get; set; }
        [JsonProperty("count_rank_ss")]
        public string CountSS { get; set; }
        [JsonProperty("count_rank_s")]
        public string CountS { get; set; }
        [JsonProperty("count_rank_a")]
        public string CountA { get; set; }
        [JsonProperty("country")]
        public string Country { get; set; }
        [JsonProperty("pp_country_rank")]
        public string PPCountryRank { get; set; }
    }
}
