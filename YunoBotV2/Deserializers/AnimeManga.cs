using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunoBotV2.Deserializers
{
    public class AnimeManga
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("title_romaji")]
        public string TitleRomaji { get; set; }
        [JsonProperty("title_english")]
        public string TitleEnglish { get; set; }
        [JsonProperty("title_japanese")]
        public string TitleJapanese { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("series_type")]
        public string SeriesType { get; set; }
        [JsonProperty("start_date")]
        public string StartDate { get; set; }
        [JsonProperty("end_date")]
        public string EndDate { get; set; }
        [JsonProperty("start_date_fuzzy")]
        public string StartDateFuzzy { get; set; }
        [JsonProperty("end_date_fuzzy")]
        public string EndDateFuzzy { get; set; }
        [JsonProperty("season")]
        public string Season { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("adult")]
        public bool Adult { get; set; }
        [JsonProperty("average_score")]
        public string AverageScore { get; set; }
        [JsonProperty("popularity")]
        public string Popularity { get; set; }
        [JsonProperty("favourite")]
        public bool Favourite { get; set; }
        [JsonProperty("image_url_sml")]
        public string ImageUrlSmall { get; set; }
        [JsonProperty("image_url_med")]
        public string ImageUrlMed { get; set; }
        [JsonProperty("image_url_lge")]
        public string ImageUrlLge { get; set; }
        [JsonProperty("image_url_banner")]
        public string ImageUrlBanner { get; set; }
        [JsonProperty("genres")]
        public string[] Genres { get; set; }
        [JsonProperty("synonyms")]
        public string[] Synonyms { get; set; }
        [JsonProperty("youtube_id")]
        public string YoutubeId { get; set; }
        [JsonProperty("hashtag")]
        public string HashTag { get; set; }
        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }
        [JsonProperty("total_episodes")]
        public string TotalEpisodes { get; set; }
        [JsonProperty("duration")]
        public string Duration { get; set; }
        [JsonProperty("airing_status")]
        public string AiringStatus { get; set; }
        [JsonProperty("source")]
        public string Source { get; set; }
        [JsonProperty("classification")]
        public string Classification { get; set; }
        //for manga
        [JsonProperty("total_chapters")]
        public string TotalChapters { get; set; }
        [JsonProperty("total_volumes")]
        public string TotalVolumes { get; set; }
        [JsonProperty("publishing_status")]
        public string PublishingStatus { get; set; }

    }
}
