using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunoBotV2.Services
{
    public static class Rand
    {
        public static Random StaticRand { get; private set; } = new Random();

        public static int Next(int exclusiveMax)
        {
            return StaticRand.Next(exclusiveMax);
        }

        public static int Next(int inclusiveMin, int exclusiveMax)
        {
            return StaticRand.Next(inclusiveMin, exclusiveMax);
        }

        public static double NextDouble(double inclusiveMin, double exclusiveMax)
        {
            return StaticRand.NextDouble() * (exclusiveMax - inclusiveMin) + inclusiveMin;
        }
    }
}
