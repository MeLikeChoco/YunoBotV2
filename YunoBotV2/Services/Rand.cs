using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunoBotV2.Services
{
    public static class Rand
    {
        private static Random _rand = new Random();

        public static int Next(int exclusiveMax)
        {
            return _rand.Next(exclusiveMax);
        }

        public static int Next(int inclusiveMin, int exclusiveMax)
        {
            return _rand.Next(inclusiveMin, exclusiveMax);
        }

        public static double NextDouble(double inclusiveMin, double exclusiveMax)
        {
            return _rand.NextDouble() * (exclusiveMax - inclusiveMin) + inclusiveMin;
        }
    }
}
