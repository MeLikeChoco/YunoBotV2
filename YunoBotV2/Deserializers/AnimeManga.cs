using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunoBotV2.Deserializers
{
    public class AnimeManga
    {
        public string id { get; set; }
        public string title_romaji { get; set; }
        public string title_english { get; set; }
        public string title_japanese { get; set; }
        public string type { get; set; }
        public string series_type { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public string start_date_fuzzy { get; set; }
        public string end_date_fuzzy { get; set; }
        public string season { get; set; }
        public string description { get; set; }
        public bool adult { get; set; }
        public string average_score { get; set; }
        public string popularity { get; set; }
        public bool favourite { get; set; }
        public string image_url_sml { get; set; }
        public string image_url_med { get; set; }
        public string image_url_lge { get; set; }
        public string image_url_banner { get; set; }
        public List<string> genres { get; set; }
        public List<string> synonyms { get; set; }
        public string youtube_id { get; set; }
        public string hashtag { get; set; }
        public string updated_at { get; set; }
        public string total_episodes { get; set; }
        public string duration { get; set; }
        public string airing_status { get; set; }
        public string source { get; set; }
        public string classification { get; set; }
        //for manga
        public string total_chapters { get; set; }
        public string total_volumes { get; set; }
        public string publishing_status { get; set; }

    }
}
