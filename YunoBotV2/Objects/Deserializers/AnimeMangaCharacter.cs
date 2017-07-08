using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunoBotV2.Objects.Deserializers
{
    public class AnimeMangaCharacter
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name_first")]
        public string NameFirst { get; set; }
        [JsonProperty("name_last")]
        public string NameLast { get; set; }
        [JsonProperty("name_japanese")]
        public string NameJapanese { get; set; }
        [JsonProperty("name_alt")]
        public string NameAlt { get; set; }
        [JsonProperty("info")]
        public string Info { get; set; }
        [JsonProperty("favourite")]
        public bool? Favourite { get; set; }
        [JsonProperty("image_url_lge")]
        public string ImageUrlLge { get; set; }
        [JsonProperty("image_url_med")]
        public string ImageUrlMed { get; set; }
    }
}
