using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunoBotV2.Deserializers
{
    public class SteamSpecialObject
    {
        public string Link { get; set; }
        public string Title { get; set; }
        public string ReleaseDate { get; set; }
        public string Rating { get; set; }
        public string Discount { get; set; }
        public string OgPrice { get; set; }
        public string NewPrice { get; set; }
        public string Picture { get; set; }
    }
}
