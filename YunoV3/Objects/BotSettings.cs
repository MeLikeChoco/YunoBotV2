using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunoV3.Objects
{
    public class BotSettings
    {

        public bool IsTest { get; private set; }

        public BotSettings()
        {

            IsTest = Environment.GetCommandLineArgs().ElementAtOrDefault(1) == "debug";

        }

    }
}
