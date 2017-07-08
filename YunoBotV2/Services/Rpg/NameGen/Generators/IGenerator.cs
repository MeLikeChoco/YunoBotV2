using System;
using System.Collections.Generic;
using System.Text;

namespace YunoBotV2.Services.Rpg.NameGen
{
    public interface IGenerator
    {

        IEnumerable<string> GeneratedResults { get; }

    }
}
