using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunoBotV2.Commands.BlackJack
{

    public class PokerCard
    {

        [JsonIgnore]
        public static readonly Dictionary<string, int> Values = new Dictionary<string, int>()
        {

            { "ACE", 11 },
            { "2", 2 },
            { "3", 3 },
            { "4", 4 },
            { "5", 5 },
            { "6", 6 },
            { "7", 7 },
            { "8", 8 },
            { "9", 9 },
            { "10", 10 },
            { "JACK", 10 },
            { "QUEEN", 10 },
            { "KING", 10 },

        };

        [JsonProperty("value")]
        public string Rank { get; set; }
        [JsonProperty("suit")]
        public string Suit { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonIgnore]
        public int Value { get { return Values[Rank]; } }

    }

}
