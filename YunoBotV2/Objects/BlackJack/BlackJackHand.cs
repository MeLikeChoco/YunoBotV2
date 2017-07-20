using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunoBotV2.Objects.BlackJack
{
    public class BlackJackHand
    {

        public List<PokerCard> PublicCards { get; private set; }
        public PokerCard SecretCard { get; private set; }

        public int Value
        {

            get
            {

                var allCards = new List<PokerCard>(PublicCards)
                {

                    SecretCard

                };
                var sum = allCards.Sum(card => card.Value);

                while (sum > 21 && allCards.Any(card => card.Rank == "ACE"))
                {

                    sum -= 10;
                    var ace = allCards.FirstOrDefault(card => card.Rank == "ACE");
                    allCards.Remove(ace);

                }

                return sum;

            }

        }

        public BlackJackHand()
        {

            PublicCards = new List<PokerCard>();

        }

        public void Start(PokerCard secret, PokerCard card)
        {

            SecretCard = secret;

            PublicCards.Add(card);

        }

        public void Add(PokerCard card)
        {

            PublicCards.Add(card);

        }

    }
}
