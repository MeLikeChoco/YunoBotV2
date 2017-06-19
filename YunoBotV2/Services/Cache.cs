using AngleSharp.Dom;
using Discord;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YunoBotV2.Configuration;
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

        public static string YelpToken;

        private static Web _service;

        public static async Task InitializeCache(Web serviceParams)
        {

            _service = serviceParams;

            await RequestYelpToken();

        }

        /// <summary>
        /// Request a new yelp api token
        /// </summary>
        public static async Task RequestYelpToken()
        {

            if (await _service.PostEncodedContent("https://api.yelp.com/oauth2/token", new KeyValuePair<string, string>[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", Config.YelpId),
                new KeyValuePair<string, string>("client_secret", Config.YelpSecret),
            }, out var result))
            {

                YelpToken = result;

            }

        }

    }
}
