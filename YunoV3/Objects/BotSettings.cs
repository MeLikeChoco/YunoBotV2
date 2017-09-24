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

            if (bool.TryParse(Environment.GetCommandLineArgs().FirstOrDefault(), out var setting))
                IsTest = setting;
            else
                IsTest = false;

        }

    }
}
