using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunoBotV2.Services
{
    public static class Cache
    {

        public static ConcurrentDictionary<ulong, Stopwatch> Stopwatches = new ConcurrentDictionary<ulong, Stopwatch>();

    }
}
