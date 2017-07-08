using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunoBotV2.Services.Rpg.NameGen
{

    [AttributeUsage(AttributeTargets.Class)]
    public class GenNameAttribute : Attribute
    {
        
        public string Name { get; set; }

        public GenNameAttribute(string name)
        {

            Name = name;

        }

    }

}
