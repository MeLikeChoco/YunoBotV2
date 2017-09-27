using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunoV3.Objects
{
    public class Tokens
    {

        public string DiscordTest { get; private set; }
        public string DiscordLegit { get; private set; }

        public string Wallpapers { get; private set; }

        private const string BaseDirectory = "Files/Tokens/";

        public Tokens()
            => Initialize();

        public void Initialize()
        {

            foreach(var file in Directory.EnumerateFiles(BaseDirectory))
            {

                switch (Path.GetFileName(file))
                {

                    case "LegitToken.txt":
                        DiscordTest = GetToken(file);
                        break;
                    case "TestToken.txt":
                        DiscordTest = GetToken(file);
                        break;
                    case "Wallpapers.txt":
                        Wallpapers = GetToken(file);
                        break;
                    default:
                        break;

                }

            }

        }

        private string GetToken(string filepath)
            => File.ReadAllText(filepath);

    }
}
