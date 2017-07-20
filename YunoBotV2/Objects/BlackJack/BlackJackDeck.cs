using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YunoBotV2.Services;
using YunoBotV2.Services.WebServices;

namespace YunoBotV2.Objects.BlackJack
{
    public class BlackJackDeck
    {

        private Web _web { get; set; }
        private string _deckId { get; set; }
        private int _remainingCards { get; set; }
        private bool _shuffled { get; set; }

        private const string DeckOfCardsApi = "https://deckofcardsapi.com/api/deck/";

        public BlackJackDeck(Web web)
        {

            _web = web;

        }

        public async Task Initialize()
        {

            var decks = Rand.Next(6, 9);
            var response = await _web.GetJObjectContent(DeckOfCardsApi + $"/new/shuffle/?deck_count={decks}");
            _deckId = response["deck_id"].ToString();
            _remainingCards = response["remaining"].ToObject<int>();
            _shuffled = response["shuffled"].ToObject<bool>();

        }

        public async Task<PokerCard> Deal()
        {

            var response = await _web.GetJObjectContent(DeckOfCardsApi + _deckId + $"/draw/?count=1");

            return response["cards"].ToObject<PokerCard>();

        }

    }
}
