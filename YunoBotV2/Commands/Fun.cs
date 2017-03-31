using Discord.Commands;
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

            JObject result = await _webService.GetJsonContent(url);

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

    }
}
