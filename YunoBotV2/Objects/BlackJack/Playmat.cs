using ImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunoBotV2.Objects.BlackJack
{
    public class Playmat
    {

        public Image<Rgba32> Cardback { get; }
        public Image<Rgba32> Table { get; }

    }
}
