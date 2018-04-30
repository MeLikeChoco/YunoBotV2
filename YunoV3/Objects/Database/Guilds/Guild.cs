using Discord.WebSocket;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            _autoRoleString = "";
            _selfRoleString = "";

        }

        public Guild() { }

        [NotMapped]
        private object _autoRoleLock = new object();
        [NotMapped]
        private object _selfRoleLock = new object();

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public ulong Id { get; set; }
        [Required]
        public string Prefix { get; set; }

        private string _autoRoleString;
        private string _selfRoleString;

        public List<ulong> AutoRoles => _autoRoleString.Split(',').Where(id => !string.IsNullOrEmpty(id)).Select(ulong.Parse).ToList();
        public List<ulong> SelfRoles => _selfRoleString.Split(',').Where(id => !string.IsNullOrEmpty(id)).Select(ulong.Parse).ToList();

        public void AddAutoRole(SocketRole role)
        {

            lock (_autoRoleLock)
                _autoRoleString = string.Join(',', AutoRoles.Union(new ulong[] { role.Id }));

        }

        public void RemoveAutoRole(SocketRole role)
        {

            lock (_autoRoleLock)
                _autoRoleString = string.Join(',', AutoRoles.Except(new ulong[] { role.Id }));

        }

        public void AddSelfRole(SocketRole role)
        {

            lock (_selfRoleLock)
                _selfRoleString = string.Join(',', SelfRoles.Union(new ulong[] { role.Id }));

        }

        public void RemoveSelfRole(SocketRole role)
        {

            lock (_selfRoleLock)
                _selfRoleString = string.Join(',', SelfRoles.Except(new ulong[] { role.Id }));

        }

    }
}
