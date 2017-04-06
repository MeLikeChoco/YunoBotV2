using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunoBotV2.Deserializers
{
    public class Rgb
    {
        public string red { get; set; }
        public string green { get; set; }
        public string blue { get; set; }
    }

    public class Hsv
    {
        public string hue { get; set; }
        public string saturation { get; set; }
    }

    public class ColorLouver
    {
        public string title { get; set; }
        public string hex { get; set; }
        public Rgb rgb { get; set; }
        public Hsv hsv { get; set; }
        public string imageUrl { get; set; }
        public string url { get; set; }
    }
}
