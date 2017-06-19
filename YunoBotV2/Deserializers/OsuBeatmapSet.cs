using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunoBotV2.Deserializers
{

    public class OsuBeatmapSet
    {

        [JsonProperty("beatmapset_id")]
        public string BeatmapSetId { get; set; }
        [JsonProperty("beatmap_id")]
        public string BeatmapId { get; set; }
        [JsonProperty("approved")]
        public string Approved { get; set; }
        [JsonProperty("total_length")]
        public string TotalLength { get; set; }
        [JsonProperty("hit_length")]
        public string HitLength { get; set; }
        [JsonProperty("version")]
        public string Version { get; set; }
        [JsonProperty("file_md5")]
        public string FileMd5 { get; set; }
        [JsonProperty("diff_size")]
        public string DiffSize { get; set; }
        [JsonProperty("diff_overall")]
        public string DiffOverall { get; set; }
        [JsonProperty("diff_approach")]
        public string DiffApproach { get; set; }
        [JsonProperty("diff_drain")]
        public string DiffDrain { get; set; }
        [JsonProperty("mode")]
        public int Mode { get; set; }
        [JsonProperty("approved_date")]
        public string ApprovedDate { get; set; }
        [JsonProperty("last_update")]
        public string LastUpdate { get; set; }
        [JsonProperty("artist")]
        public string Artist { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("creator")]
        public string Creator { get; set; }
        [JsonProperty("bpm")]
        public string BPM { get; set; }
        [JsonProperty("source")]
        public string Source { get; set; }
        [JsonProperty("tags")]
        public string Tags { get; set; }
        [JsonProperty("genre_id")]
        public string GenreId { get; set; }
        [JsonProperty("language_id")]
        public string LanguageId { get; set; }
        [JsonProperty("favourite_count")]
        public string FavouriteCount { get; set; }
        [JsonProperty("playcount")]
        public string PlayCount { get; set; }
        [JsonProperty("passcount")]
        public string PassCount { get; set; }
        [JsonProperty("max_combo")]
        public string MaxCombo { get; set; }
        [JsonProperty("difficultyrating")]
        public double DifficultyRating { get; set; }

    }
}
