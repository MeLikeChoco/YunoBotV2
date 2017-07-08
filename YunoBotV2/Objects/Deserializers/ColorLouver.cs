using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunoBotV2.Objects.Deserializers
{
    public class Rgb
    {
        [JsonProperty("red")]
        public string Red { get; set; }
        [JsonProperty("green")]
        public string Green { get; set; }
        [JsonProperty("blue")]
        public string Blue { get; set; }
    }

    public class Hsv
    {
        [JsonProperty("hue")]
        public string Hue { get; set; }
        [JsonProperty("saturation")]
        public string Saturation { get; set; }
    }

    public class ColorLouver
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("hex")]
        public string Hex { get; set; }
        [JsonProperty("rgb")]
        public Rgb RGB { get; set; }
        [JsonProperty("hsv")]
        public Hsv HSV { get; set; }
        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
