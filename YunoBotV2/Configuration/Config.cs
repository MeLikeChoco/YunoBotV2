using System;
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

        public static void LoadConfig()
        {

            var dom = XDocument.Load("Configuration/Config.xml");
            var tokens = dom.Root.Descendants("Tokens").First();
            Token = tokens.Element("Discord").Value;
            Test = tokens.Element("Test").Value;
            Osu = tokens.Element("Osu").Value;

            Game = dom.Root.Element("Game").Value;

        }

    }
}
