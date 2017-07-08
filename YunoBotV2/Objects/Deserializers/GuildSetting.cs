using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunoBotV2.Objects.Deserializers
{

    [Table("GuildSettings")]
    public class GuildSetting
    {

        [ExplicitKey]
        public ulong Id { get; set; }
        public string Prefix { get; set; }
        [Computed, Write(false)]
        public ulong[] SelfRoles { get { return SelfRoleString.Split('/').Select(str => ulong.Parse(str)).ToArray(); } }
        [Computed, Write(false)]
        public ulong[] AutoRoles { get { return AutoRoleString.Split('/').Select(str => ulong.Parse(str)).ToArray(); } }

        public string SelfRoleString { get; private set; }
        public string AutoRoleString { get; private set; }

        public GuildSetting()
        {

            SelfRoleString = "z";
            AutoRoleString = "z";

        }

        public void AddSelfRole(ulong id)
        {

            if (SelfRoleString == "z")
                SelfRoleString = SelfRoleString.Replace("z", $"{id}");
            else
                SelfRoleString += $"/{id}";

        }

        public void RemoveSelfRole(ulong id)
        {

            SelfRoleString = SelfRoleString.Replace(id.ToString(), string.Empty).Replace("//", "/");

            if (SelfRoleString.EndsWith("/"))
                SelfRoleString = SelfRoleString.Substring(0, SelfRoleString.Length - 1);

            if (string.IsNullOrEmpty(SelfRoleString))
                SelfRoleString = "z";

        }

        public void AddAutoRole(ulong id)
        {

            if (AutoRoleString == "z")
                AutoRoleString = AutoRoleString.Replace("z", $"{id}");
            else
                AutoRoleString += $"/{id}";

        }

        public void RemoveAutoRole(ulong id)
        {

            AutoRoleString = AutoRoleString.Replace(id.ToString(), string.Empty).Replace("//", "/");

            if (AutoRoleString.EndsWith("/"))
                AutoRoleString = AutoRoleString.Substring(0, AutoRoleString.Length - 1);

            if (string.IsNullOrEmpty(AutoRoleString))
                AutoRoleString = "z";

        }

    }

}