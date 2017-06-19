using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunoBotV2.Deserializers
{
    public class Recipe
    {
        [JsonProperty("uri")]
        public string Uri { get; set; }
        [JsonProperty("label")]
        public string Label { get; set; }
        [JsonProperty("image")]
        public string Image { get; set; }
        [JsonProperty("source")]
        public string Source { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("shareAs")]
        public string ShareAs { get; set; }
        [JsonProperty("yield")]
        public string Yield { get; set; } //aka servings
        [JsonProperty("dietLabels")]
        public string[] DietLabels { get; set; }
        [JsonProperty("healthLabels")]
        public string[] HealthLabels { get; set; }
        [JsonProperty("cautions")]
        public string[] Cautions { get; set; }
        [JsonProperty("ingredientLines")]
        public string[] Ingredients { get; set; }
        [JsonProperty("calories")]
        public string Calories { get; set; }
        [JsonProperty("totalWeight")]
        public string TotalWeight { get; set; }
    }
}
