using AngleSharp.Dom;
using Discord;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YunoBotV2.Deserializers;
using YunoBotV2.Services.WebServices;

namespace YunoBotV2.Services
{
    public static class Cache
    {

        public static ConcurrentDictionary<ulong, Stopwatch> Stopwatches = new ConcurrentDictionary<ulong, Stopwatch>();
                
        public static EmbedBuilder SteamSpecials = new EmbedBuilder();
        public static DateTime LastSteamSpecialScrape = new DateTime(1990, 1, 1, 1, 1, 1, 1);

        public static EmbedBuilder SteamTopSellers = new EmbedBuilder();
        public static DateTime LastSteamTopSellerScrape = new DateTime(1990, 1, 1, 1, 1, 1, 1);

        public static EmbedBuilder SteamNewReleases = new EmbedBuilder();
        public static DateTime LastSteamNewReleasesScrape = new DateTime(1990, 1, 1, 1, 1, 1, 1);

        public static Dictionary<string, EmbedBuilder> Pokemon = new Dictionary<string, EmbedBuilder>();

        public static Task InitializeCache(Web service)
        {

            return Task.CompletedTask;

        }

    }
}
