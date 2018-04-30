using Discord;
using Discord.Commands;
using Discord.WebSocket;
using MoreLinq;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using YunoV3.Extensions;
using YunoV3.Objects;
using YunoV3.Services;

namespace YunoV3.Modules.Commands
{
    [RequireContext(ContextType.Guild)]
    public class Battle : CustomBase
    {

        public Web Web { get; set; }
        public Random Random { get; set; }
        public BotSettings BotSettings { get; set; }

        private string[] _battleMoves;

        protected override void BeforeExecute(CommandInfo command)
            => _battleMoves = BotSettings.BattleMoves;

        [Command("battle")]
        [Summary("Battle a random user to the death")]
        public Task BattleSomeoneRandom()
            => BattleSomeone(Context.Guild.Users
                .Where(user => user.Username != Context.User.Username)
                .RandomSubset(1).First());

        [Command("battle")]
        [Summary("Battle someone to the death")]
        public async Task BattleSomeone(SocketGuildUser enemy)
        {
            
            var user = Context.User as SocketGuildUser;
            var bUser = new BattleUser(user);
            var bEnemy = new BattleUser(enemy);

            using (var userStream = await GetAvatarStream(user))
            using (var enemyStream = await GetAvatarStream(enemy))
            using (var baseImage = SixLabors.ImageSharp.Image.Load("Files/Images/deathbattle.png"))
            using (var userImage = SixLabors.ImageSharp.Image.Load(userStream))
            using (var enemyImage = SixLabors.ImageSharp.Image.Load(enemyStream))
            using (var output = new MemoryStream())
            {

                userImage.Resize(359, 376);
                enemyImage.Resize(359, 376);

                Font font;

                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                    font = new Font(SystemFonts.Find("Segoe UI"), 30, FontStyle.Bold);
                else
                    font = new Font(SystemFonts.Find("DejaVu Sans"), 25, FontStyle.Bold);

                var textOptions = new TextGraphicsOptions
                {

                    Antialias = true,                    
                    ApplyKerning = true,                    
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center

                };

                var pngEncoder = new PngEncoder
                {

                    CompressionLevel = 1,
                    IgnoreMetadata = true,

                };

                baseImage.Mutate(processor =>
                {

                    processor.DrawImage(userImage, 1, Size.Empty, new Point(21, 193));
                    processor.DrawImage(enemyImage, 1, Size.Empty, new Point(520, 193));
                    processor.DrawText(bUser.Name, font, Rgba32.Green, new Vector2(195, 606), textOptions);
                    processor.DrawText(bEnemy.Name, font, Rgba32.Red, new Vector2(700, 606), textOptions);

                });

                baseImage.SaveAsPng(output, pngEncoder);
                output.Seek(0, SeekOrigin.Begin);

                await UploadAsync(output, "deathbattle.png");

            }

            var log = new StringBuilder();
            Attacker attacker;
            int userRoll, enemyRoll;

            do
            {

                userRoll = Random.Next(1, 101);
                enemyRoll = Random.Next(1, 101);

            } while (userRoll == enemyRoll); //highly unlikely, but might as well be sure

            attacker = userRoll > enemyRoll ? Attacker.User : Attacker.Enemy;

            log.AppendLine($":game_die: **{bUser}** rolled __{userRoll}__. **{bEnemy}** rolled __{enemyRoll}__.");

            if (attacker == Attacker.User)
                log.Append($"**{bUser}** ");
            else
                log.Append($"**{bEnemy}** ");

            log.AppendLine(" got a higher roll. He/she initiates the first attack!");
            
            var fight = await SendEmbedAsync(GetEmbed(log.ToString(), bUser, bEnemy, attacker));
            BattleUser winner;

            do
            {

                await Task.Delay(2000);

                var damage = Random.Next(3, 29);
                var battleline = _battleMoves[Random.Next(0, _battleMoves.Length)];

                if (attacker == Attacker.User)
                {

                    bEnemy.Damage(damage);
                    log.AppendLine($":arrow_right: {string.Format(battleline, $"**{bUser}**", $"**{bEnemy}**", $"__{damage}__")}");

                }
                else
                {

                    bUser.Damage(damage);
                    log.AppendLine($":arrow_left: {string.Format(battleline, $"**{bEnemy}**", $"**{bUser}**", $"__{damage}__")}");

                }

                await fight.EditAsync(GetEmbed(log.ToString(), bUser, bEnemy, attacker));

                if (attacker == Attacker.User)
                    attacker = Attacker.Enemy;
                else
                    attacker = Attacker.User;

            } while (bUser.Hp != 0 && bEnemy.Hp != 0);

            if (bUser.Hp == 0)
                winner = bEnemy;
            else
                winner = bUser;

            log.Append($":trophy: **{winner}** has won the bout!");
            await fight.EditAsync(GetEmbed(log.ToString(), bUser, bEnemy, Attacker.Winner));

        }

        private async Task<Stream> GetAvatarStream(SocketGuildUser user)
        {

            if (user.GetAvatarUrl() != null)
                return (await Web.GetStreamAsync(user.GetAvatarUrl())).stream;
            else
            {

                var filepath = Directory.GetFiles("Files/Images/Default Discord Avatars/").RandomSubset(1).First();
                return File.Open(filepath, FileMode.Open);

            }

        }

        private Embed GetEmbed(string battlelog, BattleUser user, BattleUser enemy, Attacker attacker)
        {

            Color color;

            switch (attacker)
            {

                case Attacker.Enemy:
                    color = new Color(255, 0, 0);
                    break;
                case Attacker.User:
                    color = new Color(0, 128, 0);
                    break;
                case Attacker.Winner:
                default:
                    color = new Color(255, 215, 0);
                    break;

            }            

            return new EmbedBuilder()
                .WithColor(color)
                .WithDescription(battlelog)
                .AddField(user.Name, $"{user.Hp} / 100", true)
                .AddField(enemy.Name, $"{enemy.Hp} / 100", true)
                .Build();

        }

        private enum Attacker
        {

            User,
            Enemy,
            Winner

        }

    }
}
