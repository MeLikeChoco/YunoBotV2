﻿using AngleSharp.Dom;
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

        public static List<SteamSpecialObject> SteamSpecials = new List<SteamSpecialObject>(10);
        public static DateTime LastSteamSpecialScrape = new DateTime(1990, 1, 1, 1, 1, 1, 1);

        public static Dictionary<string, EmbedBuilder> Pokemon = new Dictionary<string, EmbedBuilder>();

        public static async Task InitializeCache(Web service)
        {

            

        }

    }
}
