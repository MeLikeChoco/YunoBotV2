﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace YunoBotV2.Configuration
{
    public static class Config
    {

        public static string Token { get; private set; }
        public static string Test { get; private set; }
        public static string Game { get; private set; }
        public static string Osu { get; private set; }
        public static string EdamamId { get; private set; }
        public static string EdamamKey { get; private set; }
        public static string ChampionGG { get; private set; }
        public static string LeagueOfLegends { get; private set; }

        public static void LoadConfig()
        {

            var dom = XDocument.Load("Configuration/Config.xml");
            XElement tokens = dom.Root.Descendants("Tokens").First();
            XElement edamam = tokens.Descendants("Edamam").First();

            Token = tokens.Element("Discord").Value;
            Test = tokens.Element("Test").Value;
            Osu = tokens.Element("Osu").Value;
            ChampionGG = tokens.Element("Championgg").Value;
            LeagueOfLegends = tokens.Element("LeagueOfLegends").Value;

            EdamamId = edamam.Element("appid").Value;
            EdamamKey = edamam.Element("appkey").Value;

            Game = dom.Root.Element("Game").Value;

        }

    }
}
