using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YunoBotV2.Services.WebServices;

namespace YunoBotV2.Commands
{
    public class Fun : CustomModuleBase
    {

        private WebService _webService;
        private Unshortener _unshortenService;

        public Fun(WebService webServiceParams, Unshortener unshortenParams)
        {

            _webService = webServiceParams;
            _unshortenService = unshortenParams;

        }

        [Command("ping")]
        [Summary("Returns the ping between bot and guild")]
        [RequireContext(ContextType.Guild)]
        public async Task PingCommand()
            => await ReplyAsync($"The ping between bot and guild is: **{(Context.Client.Latency)} ms**");

        [Command("gif")]
        [Summary("Returns a gif from giphy")]
        public async Task GifCommand([Remainder]string search)
        {

            //api key is the public one
            var url = "http://api.giphy.com/v1/gifs/random?api_key=dc6zaTOxFJmzC&tag=" + search;
            JObject result = await _webService.GetJObjectContent(url);
            var gifUrl = result["data"]["image_original_url"].ToString();

            await ReplyAsync(gifUrl);            

        }

        [Command("norris")]
        [Summary("Returns a really bad Chuck Norris joke, with optional name!")]
        public async Task NorrisCommand([Remainder]string name = null)
        {

            if (string.IsNullOrEmpty(name)) name = "Chuck Norris";

            string url;

            if((name.IndexOf(' ') == name.LastIndexOf(' ')) && name.IndexOf(' ') != -1) 
            {

                string[] temp = name.Split(' ');
                url = $"http://api.icndb.com/jokes/random?firstName={Uri.EscapeUriString(temp[0])}&lastName={Uri.EscapeUriString(temp[1])}";

            }
            else url = $"http://api.icndb.com/jokes/random?firstName={Uri.EscapeUriString(name)}&lastName=";

            JObject result = await _webService.GetJObjectContent(url);

            if(result == null)
            {
                await DefaultErrorMessage();
                return;
            }
            //replace the extra space after name
            //due to lack of last name
            else await ReplyAsync(Regex.Replace((string)result["value"]["joke"], @"\s+", " "));            

        }

        [Command("for the glory of satan")]
        [Summary("WHY? FOR THE GLORY OF SATAN OF COURSE")]
        public async Task SatanCommand()
            => await ReplyAsync("http://i3.kym-cdn.com/photos/images/newsfeed/000/613/025/b64.jpg");

        [Command("support")]
        [Summary("Gets the support server invite")]
        public async Task SupportCommand()
            => await ReplyAsync("IT SEEMS YOU NEED HEALIN'\nhttps://discord.gg/UPxvDKe");

        [Command("roll")]
        [Summary("Roll a dice or a blunt")]
        public async Task RollCommand(string sides = "6")
        {

            if (sides.Equals("blunt"))
            {

                await ReplyAsync(":smoking:");

            }
            else if (int.TryParse(sides, out int sidesOfDice))
            {

                var diceRollDrumRollPlease = new Random().Next(1, sidesOfDice + 1);
                await ReplyAsync($":game_die: You have rolled {diceRollDrumRollPlease}");

            }
            else await ReplyAsync("That ain't a number");

        }

        [Command("coinflip")]
        [Summary("Flip a coin...like this shit hasn't been done to death")]
        public async Task CoinFlipCommand()
        {

            var r = new Random().Next(2);

            switch (r)
            {

                case 0:
                    await ReplyAsync("HEADS");
                    break;
                case 1:
                    await ReplyAsync("TAILS");
                    break;

            }

        }

        [Command("uptime")]
        [Summary("Return the uptime of the bot")]
        public async Task UptimeCommand()
        {

            var startTime = DateTime.UtcNow.Subtract(Process.GetCurrentProcess().StartTime.ToUniversalTime());
            var randomNumber = new Random().Next(1, 10);
            string emoji;

            switch (randomNumber)
            {
                case 1:
                    emoji = ":clock1:";
                    break;
                case 2:
                    emoji = ":clock10:";
                    break;
                case 3:
                    emoji = ":clock1030:";
                    break;
                case 4:
                    emoji = ":clock11:";
                    break;
                case 5:
                    emoji = ":clock1130:";
                    break;
                case 6:
                    emoji = ":clock12:";
                    break;
                case 7:
                    emoji = ":clock1230:";
                    break;
                case 8:
                    emoji = ":clock130:";
                    break;
                case 9:
                    emoji = ":clock2:";
                    break;
                default:
                    emoji = ":clock:";
                    break;
            }

            await ReplyAsync($"{emoji} {startTime.Days} days {startTime.Hours} hours {startTime.Minutes} minutes {startTime.Seconds} seconds");

        }

        [Command("ascii")]
        [Summary("Text -> Ascii art")]
        public async Task AsciiCommand([Remainder]string text)
        {

            var url = $"http://artii.herokuapp.com/make?text={Uri.EscapeUriString(text)}";
            string result = await _webService.GetRawContent(url);

            if (string.IsNullOrEmpty(result)) await DefaultErrorMessage();
            else await ReplyAsync($"```{result}```");

        }

        [Command("unshorten")]
        [Summary("Unshorten a link for it's info")]
        public async Task UnshortenCommand([Remainder]string url)
        {

            if (url.Contains("adf.ly"))
                await ReplyAsync("With how adf.ly works, I cannot determine the correct end link.");
            else
                await ReplyAsync($"{Context.User.Mention} The actual link is: {await _unshortenService.Get(url)}");

        }

        [Command("rr")]
        [Summary("Play a round of russian roulette")]
        public async Task RussianRouletteCommand(int bullets = 1, int capacity = 6)
        {

            if (bullets > capacity)
            {

                await ReplyAsync($"You try fitting {bullets} bullets in a {capacity} round gun and see if that works.");
                return;

            }

            var r = new Random();
            var landingPos = r.Next(1, capacity + 1);
            var bulletPositions = new int[bullets];

            for (int i = 0; i < bullets; i++)
            {               

                while (true)
                {

                    var b = r.Next(1, capacity + 1);
                    if (!(bulletPositions.Contains(b)))
                    {

                        bulletPositions[i] = b;
                        break;

                    }

                }

            }

            if (bulletPositions.Contains(landingPos))
                await ReplyAsync(":gun: BAM, YOU DEAD :skull:");
            else
                await ReplyAsync($"{Context.User.Mention} :gun: You live to see another day which you will spend browsing the internet " +
                    "wishing you were doing something else productive.");

        }

        [Command("invite")]
        [Summary("Get an invite link for this bot!")]
        public async Task InviteCommand()
            => await ReplyAsync($"{Context.User.Mention} https://discordapp.com/oauth2/authorize?client_id={Context.Client.GetApplicationInfoAsync().Result.Id}&scope=bot&permissions=0" +
                "\nPlease take care of me.");

        [Command("you suck")]
        [Summary("Insult the bot")]
        public async Task YouSuckCommand()
            => await ReplyAsync($"{Context.User.Mention} I think it's hilarious you kids talking shit about me. " +
                "You wouldn't say shit to me irl, " + $"I'm jacked. Not only that but I wear the freshest clothes, " +
                     "eat at the chillest restaurants, and bang the hottest dudes. Ya'll are pathetic lol.﻿");

    }
}
