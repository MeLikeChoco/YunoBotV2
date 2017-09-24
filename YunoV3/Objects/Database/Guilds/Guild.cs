using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunoV3.Objects.Database.Guilds
{
    public class Guild
    {

        public Guild(ulong id)
        {

            Id = id;
            Prefix = "e$";

        }

        public Guild() { }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public ulong Id { get; set; }
        [Required]
        public string Prefix { get; set; }
        [Column("AutoRoles")]
        public string AutoRoleString { get; private set; }
        [Column("SelfRoles")]
        public string SelfRoleString { get; private set; }

        [NotMapped]
        public List<ulong> AutoRoles
        {

            get => new List<ulong>(AutoRoleString.Split(',').Select(id => ulong.Parse(id)));

        }

        [NotMapped]
        public List<ulong> SelfRoles
        {
            
            get => new List<ulong>(SelfRoleString.Split(',').Select(id => ulong.Parse(id)));

        }

        [NotMapped]
        private object _autoRoleLock = new object();

        public void AddAutoRole(SocketRole role)
        {

            lock (_autoRoleLock)
            {

                if (string.IsNullOrEmpty(AutoRoleString))
                    AutoRoleString = role.Id.ToString();
                else
                    AutoRoleString += $",{role.Id}";

            }

        }

        public void RemoveAutoRole(SocketRole role)
        {

            lock (_autoRoleLock)
            {

                if (!string.IsNullOrEmpty(AutoRoleString))
                    AutoRoleString = AutoRoleString.Replace(role.Id.ToString(), "").Replace(",,", "");

            }

        }

    }
}
