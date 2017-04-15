using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunoBotV2.Deserializers
{
    public class AnimeMangaCharacter
    {
        public string id { get; set; }
        public string name_first { get; set; }
        public string name_last { get; set; }
        public string name_japanese { get; set; }
        public string name_alt { get; set; }
        public string info { get; set; }
        public bool? favourite { get; set; }
        public string image_url_lge { get; set; }
        public string image_url_med { get; set; }
    }
}
