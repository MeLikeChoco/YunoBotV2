using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunoBotV2.Objects.BlackJack
{
    public class BlackJackPlayer
    {

        public ulong UserId { get; private set; }
        public BlackJackHand Hand { get; private set; }

        public BlackJackPlayer(SocketUser user)
        {

            UserId = user.Id;
            Hand = new BlackJackHand();

        }

    }
}
