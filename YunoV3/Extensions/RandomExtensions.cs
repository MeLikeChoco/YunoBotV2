using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunoV3.Extensions
{
    public static class RandomExtensions
    {

        public static Color GetColor(this Random random)
            => new Color(random.Next(1, 256), random.Next(1, 256), random.Next(1, 256));

    }
}
