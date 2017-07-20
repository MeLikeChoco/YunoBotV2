using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YunoBotV2.Services.WebServices;
using YunoBotV2.Objects.BlackJack;

namespace YunoBotV2.Commands
{
    public class BlackJack : CustomModuleBase
    {

        private Web _web { get; set; }
        private List<BlackJackPlayer> _players { get; set; }
        private BlackJackDeck _deck { get; set; }

        protected override void BeforeExecute(CommandInfo command)
        {

            _players = new List<BlackJackPlayer>(7)
            {

                new BlackJackPlayer(Context.User),
                new BlackJackPlayer(Context.Client.CurrentUser)

            };

        }

        public BlackJack(Web web)
        {

            _web = web;

        }

        [Command("blackjack")]
        [Summary("Play a blackjack game!")]
        public async Task BlackJackCommand()
        {

            var _deck = new BlackJackDeck(_web);
            var deckInt = _deck.Initialize();

            await ReplyAsync("5 slots for blackjack left! Type `join` to play!");
            Context.Client.MessageReceived += RegisterUser;

            await Task.Delay(30000);

            Context.Client.MessageReceived -= RegisterUser;
            await deckInt.ConfigureAwait(false);



        }

        private async Task RegisterUser(SocketMessage message)
        {

            if (message.Channel.Id == Context.Channel.Id && message.Content.ToLower() == "join")
            {

                var user = message.Author;

                _players.Add(new BlackJackPlayer(user));

                await ReplyAsync($"{7 - _players.Count} slots for blackjack left! Type `join` to play!");

            }

        }

    }
}
