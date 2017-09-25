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
            _autoRoleString = "";
            _selfRoleString = "";

        }

        public Guild() { }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public ulong Id { get; set; }
        [Required]
        public string Prefix { get; set; }
        [Column("AutoRoles")]
        [Required]
        public string _autoRoleString { get; private set; }
        [Column("SelfRoles")]
        [Required]
        public string _selfRoleString { get; private set; }

        [NotMapped]
        public List<ulong> AutoRoles
        {

            //there where method is just in case the string is empty, so i can return an empty collection instead of throwing an error
            get => new List<ulong>(_autoRoleString.Split(',').Where(id => id != "").Select(id => ulong.Parse(id)));

        }

        [NotMapped]
        public List<ulong> SelfRoles
        {
            
            get => new List<ulong>(_selfRoleString.Split(',').Where(id => id != "").Select(id => ulong.Parse(id)));

        }

        public void AddAutoRole(SocketRole role)
        {

            if (string.IsNullOrEmpty(_autoRoleString))
                _autoRoleString = role.Id.ToString();
            else if (!_autoRoleString.Contains(role.Id.ToString()))
                _autoRoleString += $",{role.Id}";

        }

        public void RemoveAutoRole(SocketRole role)
        {

            if (!string.IsNullOrEmpty(_autoRoleString))
                _autoRoleString = _autoRoleString.Replace(role.Id.ToString(), "").Replace(",,", "");

        }

        public void AddSelfRole(SocketRole role)
        {

            if (string.IsNullOrEmpty(_selfRoleString))
                _selfRoleString = role.Id.ToString();
            else
                _selfRoleString += $",{role.Id}";

        }

        public void RemoveSelfRole(SocketRole role)
        {

            if (!string.IsNullOrEmpty(_selfRoleString))
                _selfRoleString = _selfRoleString.Replace(role.Id.ToString(), "").Replace(",,", "");

        }

    }
}
