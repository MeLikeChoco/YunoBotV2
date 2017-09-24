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

        public Tokens()
        {

            DiscordTest = File.ReadAllText("Files/TestToken.txt");
            DiscordLegit = File.ReadAllText("Files/LegitToken.txt");

        }

    }
}
