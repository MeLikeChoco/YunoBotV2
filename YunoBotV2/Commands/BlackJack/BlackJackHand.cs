using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunoBotV2.Commands.BlackJack
{
    public class BlackJackHand
    {

        public List<PokerCard> PublicCards { get; private set; }
        public PokerCard SecretCard { get; private set; }

        public BlackJackHand()
        {

            PublicCards = new List<PokerCard>();

        }

        public void Deal(PokerCard card)
        {

            PublicCards.Add(card);

        }

    }
}
