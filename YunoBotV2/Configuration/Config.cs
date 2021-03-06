﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using YunoBotV2.Services;

namespace YunoBotV2.Configuration
{
    public static class Config
    {

        public static string Token { get; private set; }
        public static string Test { get; private set; }
        public static string Game { get; private set; }
        public static string Osu { get; private set; }
        public static string ChampionGG { get; private set; }
        public static string LeagueOfLegends { get; private set; }
        public static string WallpaperAlphaCoders { get; private set; }
        public static string YandexTranslate { get; private set; }
        public static string RandomAPI { get; private set; }
        public static string Thesaurus { get; private set; }
        public static string GooglePlaces { get; private set; }

        public static string YelpId { get; private set; }
        public static string YelpSecret { get; private set; }

        public static string EdamamId { get; private set; }
        public static string EdamamKey { get; private set; }

        public static string AnilistId { get; private set; }
        public static string AnilistSecret { get; private set; }

        public static string AnilistToken { get; set; } = "tehee";

        public static string[] BattleMoves { get; private set; }

        public static void LoadConfig()
        {

            var dom = XDocument.Load("Configuration/Config.xml");
            XElement tokens = dom.Root.Descendants("Tokens").First();
            XElement yelp = tokens.Descendants("Yelp").First();
            XElement edamam = tokens.Descendants("Edamam").First();
            XElement anilist = tokens.Descendants("Anilist").First();

            Token = tokens.Element("Discord").Value;
            Test = tokens.Element("Test").Value;
            Osu = tokens.Element("Osu").Value;
            ChampionGG = tokens.Element("Championgg").Value;
            LeagueOfLegends = tokens.Element("LeagueOfLegends").Value;
            WallpaperAlphaCoders = tokens.Element("Wallpaper").Value;
            YandexTranslate = tokens.Element("Yandex").Value;
            RandomAPI = tokens.Element("Random").Value;
            Thesaurus = tokens.Element("Thesaurus").Value;
            GooglePlaces = tokens.Element("GooglePlaces").Value;

            YelpId = yelp.Element("clientid").Value;
            YelpSecret = yelp.Element("clientsecret").Value;

            AnilistId = anilist.Element("clientid").Value;
            AnilistSecret = anilist.Element("clientsecret").Value;

            EdamamId = edamam.Element("appid").Value;
            EdamamKey = edamam.Element("appkey").Value;

            Game = dom.Root.Element("Game").Value;

            BattleMoves = File.ReadAllLines("Configuration/BattleMoves.txt");
            for (int i = 0; i < BattleMoves.Length; i++)
                BattleMoves[i] = BattleMoves[i].Replace("%d", "__%d__"); //im lazy ;_;

            //shuffle BattleMoves
            for(int i = 0; i < BattleMoves.Length; i++)
            {
                var newIndex = Rand.Next(0, BattleMoves.Length);

                if (i == newIndex)
                    continue;

                var temp = BattleMoves[newIndex];
                BattleMoves[newIndex] = BattleMoves[i];
                BattleMoves[i] = temp;
            }

        }

    }
}
