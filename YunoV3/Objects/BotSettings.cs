using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunoV3.Objects
{
    public class BotSettings
    {

        public bool IsTest { get; private set; }
        public string[] BattleMoves { get; private set; }

        public BotSettings()
            => Initialize();

        public void Initialize()
        {

            IsTest = Environment.GetCommandLineArgs().ElementAtOrDefault(1) == "debug";
            BattleMoves = File.ReadAllLines("Files/BattleMoves.txt");

        }

    }
}
