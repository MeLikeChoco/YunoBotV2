using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunoV3.Objects
{
    public class BattleUser
    {

        public string Name { get; }
        public int Hp { get; private set; }

        public BattleUser(SocketGuildUser user)
            : this(user, 100) { }

        public BattleUser(SocketGuildUser user, int hp)
        {

            Name = user.Nickname ?? user.Username;
            Hp = hp;

        }

        public void Damage(int damage)
            => Hp = Math.Max(0, Hp -= damage);

        public override string ToString()
            => Name;

    }
}
