using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using YunoBotV2.Commands.Attributes;
using YunoBotV2.Configuration;
using YunoBotV2.Core;
using YunoBotV2.Services.WebServices;
using ImageSharp;
using SixLabors.Fonts;
using System.Numerics;
using ImageSharp.Drawing;
using YunoBotV2.Services;
using MoreLinq;
using SixLabors.Primitives;
using ImageSharp.Formats;

namespace YunoBotV2.Commands
{
    public class Battle : CustomModuleBase
    {

        private Web _service;
        private string _leftUser, _rightUser, _formattedLeft, _formattedRight;
        SocketGuildUser _rightUserObject;
        private Color _color;
        //it's overkill, but I wanted to try it out
        private RandomNumberGenerator _rand;

        public Battle(Web serviceParams)
            => _service = serviceParams;

        protected override void BeforeExecute(CommandInfo cmdInfo)
        {
            //_leftUser = Context.User.Username;
            _leftUser = (Context.User as IGuildUser).Nickname ?? Context.User.Username;
            _formattedLeft = $"**{@_leftUser}**";
            _rand = RandomNumberGenerator.Create();
        }

        [Command("battle")]
        [Summary("Battle someone")]
        [RequireContext(ContextType.Guild)]
        [Cooldown(40)]
        public async Task BattleCommand(SocketGuildUser user) //accepts id and mention
        {

            //_rightUser = user.Username;
            _rightUserObject = user;
            _rightUser = (user as IGuildUser).Nickname ?? user.Username;
            _formattedRight = $"**{_rightUser}**";
            try
            {
                await SendImage(user);
                await StartBattle();
            }
            catch (Exception e)
            {
                AltConsole.Print(e.StackTrace);
            }

        }

        [Command("battler")]
        [Summary("Battle someone")]
        [RequireContext(ContextType.Guild)]
        public async Task RandomBattleCommand()
        {
            
            _rightUserObject = Context.Guild.Users.Where(u => u.Status != UserStatus.Offline).RandomSubset(1, Rand.StaticRand).First();
            _rightUser = _rightUserObject.Nickname ?? _rightUserObject.Username;
            _formattedRight = $"**{_rightUser}**";
            await SendImage(_rightUserObject);

            //await StartBattle();

        }

        private async Task SendImage(SocketGuildUser rightUser)
        {

            var leftUserAvatar = Context.User.GetAvatarUrl();
            var rightUserAvatar = rightUser.GetAvatarUrl();

            //ImageSharp
            using (var baseImage = ImageSharp.Image.Load("Configuration/deathbattle.png"))
            using (var leftImage = ImageSharp.Image.Load(await _service.GetStream(leftUserAvatar)).Resize(246, 262))
            using (var rightImage = ImageSharp.Image.Load(await _service.GetStream(rightUserAvatar)).Resize(246, 262))
            using (var stream = new MemoryStream())
            {
                
                var size = new Size();
                var font = new Font(SystemFonts.Find("DejaVu Sans"), 20);
                var attacker = new Rgba32(0, 128, 0);
                var defender = new Rgba32(220, 20, 60);

                var textOptions = new TextGraphicsOptions
                {
                    ApplyKerning = true,
                    Antialias = true,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                baseImage.DrawImage(leftImage, 1, size, new Point(15, 132));
                baseImage.DrawImage(rightImage, 1, size, new Point(359, 132));

                baseImage.DrawText(_leftUser, font, attacker, new Vector2(128, 406), textOptions);
                baseImage.DrawText(_rightUser, font, defender, new Vector2(480, 406), textOptions);

                baseImage.SaveAsPng(stream);
                stream.Position = 0; //reset position to write to sendfileasync

                await UploadAsync(stream, "png");

            }

            //System.Drawing (corecompat)
            //stupid discord also uses the struct Image
            //using (var canvas = new Bitmap(System.Drawing.Image.FromFile("Configuration/deathbattle.png")))
            //using (var baseImage = Graphics.FromImage(canvas))
            //using (Bitmap leftImage = ResizeImage(System.Drawing.Image.FromStream(await _service.GetStream(leftUserAvatar)), 246, 262))
            //using (Bitmap rightImage = ResizeImage(System.Drawing.Image.FromStream(await _service.GetStream(rightUserAvatar)), 246, 262))
            //using (var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            //using (var font = new Font(FontFamily.GenericSansSerif, 14))
            //using (var stream = new MemoryStream())
            //{

            //    baseImage.DrawImage(leftImage, new Point(15, 132));
            //    baseImage.DrawImage(rightImage, new Point(359, 132));

            //    var leftRect = new Rectangle(6, 404, 256, 34);
            //    var rightRect = new Rectangle(355, 404, 254, 34);

            //    baseImage.DrawString(_leftUser, font, Brushes.Black, new Point(128, 421), format);
            //    baseImage.DrawString(_rightUser, font, Brushes.Black, new Point(477, 421), format);
            //    //baseImage.DrawString(_leftUser, font, Brushes.Black, leftRect, format);
            //    //baseImage.DrawString(_rightUser, font, Brushes.Black, rightRect, format);
            //    //baseImage.DrawRectangle(Pens.Black, leftRect);
            //    //baseImage.DrawRectangle(Pens.Black, rightRect);

            //    canvas.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
            //    stream.Position = 0; //reset position to write to sendfileasync

            //    await Context.Channel.SendFileAsync(stream, "jpeg");

            //}

        }

        private async Task StartBattle()
        {

            var leftHealth = 100;
            var rightHealth = 100;
            _color = new Color(Next(0, 256), Next(0, 256), Next(0, 256));
            var messageLines = new StringBuilder();
            string battleMessage;
            bool leftAttackRight;

            EmbedBuilder eBuilder;
            int leftDice, rightDice;
            IUserMessage message;

            do
            {
                leftDice = Next(1, 7);
                rightDice = Next(1, 7);
            } while (leftDice == rightDice);

            leftAttackRight = leftDice > rightDice ? true : false;
            var initiator = leftDice > rightDice ? _formattedLeft : _formattedRight;

            messageLines.AppendLine($":game_die: {_formattedLeft} rolls a {leftDice}. {_formattedRight} rolls a {rightDice}.");
            messageLines.AppendLine($"{initiator} got a higher roll. He/she initiates the attack");

            eBuilder = GetEmbed(messageLines.ToString(), leftHealth, rightHealth);
            message = await ReplyAsync("", embed: eBuilder);

            do
            {

                battleMessage = Config.BattleMoves[Next(0, Config.BattleMoves.Length)];
                int damage = Next(3, 34);
                string attacker, victim, emoji;

                if (leftAttackRight)
                {
                    rightHealth = (rightHealth - damage) < 0 ? 0 : rightHealth - damage;
                    attacker = _formattedLeft;
                    victim = _formattedRight;
                    emoji = ":arrow_right:";
                }
                else
                {

                    if (_rightUserObject.Id == 290622876657385473)
                        damage = 100;

                    leftHealth = (leftHealth - damage) < 0 ? 0 : leftHealth - damage;
                    attacker = _formattedRight;
                    victim = _formattedLeft;
                    emoji = ":arrow_left:";
                }

                messageLines.AppendLine($"{emoji} {battleMessage.Replace("%a", attacker).Replace("%v", victim).Replace("%d", damage.ToString())}");

                eBuilder = GetEmbed(messageLines.ToString(), leftHealth, rightHealth);
                await message.ModifyAsync(property => property.Embed = eBuilder.Build());

                //break straight away if one of them becomes 0
                if (leftHealth == 0 || rightHealth == 0)
                    break;

                leftAttackRight = !leftAttackRight; //yes, there's also leftAttackRight ^= true, but I like readability

                await Task.Delay(3000); //whatever, rate limit is 5 edits per 5 seconds (NOT 1 EDIT PER 1 SECOND), a little more than 1 second delay will be fine

            } while (leftHealth > 0 && rightHealth > 0);

            string winner = leftHealth > rightHealth ? _formattedLeft : _formattedRight;
            messageLines.AppendLine($":trophy: {winner} is the winner of this fight!");

            eBuilder = GetEmbed(messageLines.ToString(), leftHealth, rightHealth);
            await message.ModifyAsync(property => property.Embed = eBuilder.Build());

        }

        private int Next(int inclusiveMin, int exclusiveMax)
        {
            // Generate four random bytes
            byte[] four_bytes = new byte[4];
            _rand.GetBytes(four_bytes);

            // Convert the bytes to a UInt32
            UInt32 scale = BitConverter.ToUInt32(four_bytes, 0);

            // And use that to pick a random number >= min and < max
            return (int)(inclusiveMin + (exclusiveMax - inclusiveMin) * (scale / (uint.MaxValue + 1.0)));
        }

        private EmbedBuilder GetEmbed(string message, int leftHealth, int rightHealth)
        {
            return new EmbedBuilder().WithColor(_color).WithDescription(message).AddInlineField(_leftUser, $"{leftHealth} / 100").AddInlineField(_rightUser, $"{rightHealth} / 100");
        }

        public string CenterString(string source, int length)
        {
            int spaces = length - source.Length;
            int padLeft = spaces / 2 + source.Length;

            return source.PadLeft(padLeft).PadRight(length);
        }

        /// <summary>
        /// Resize image while having good quality, well, attempt to have good quality
        /// </summary>
        /// <param name="image">The image to resize</param>
        /// <param name="width">The desired width</param>
        /// <param name="height">The desired height</param>
        /// <returns>Bitmap</returns>
        //private Bitmap ResizeImage(System.Drawing.Image image, int width, int height)
        //{
        //    var destRect = new System.Drawing.Rectangle(0, 0, width, height);
        //    var destImage = new Bitmap(width, height);

        //    destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

        //    using (var graphics = Graphics.FromImage(destImage))
        //    {
        //        graphics.CompositingMode = CompositingMode.SourceCopy;
        //        graphics.CompositingQuality = CompositingQuality.HighQuality;
        //        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //        graphics.SmoothingMode = SmoothingMode.HighQuality;
        //        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

        //        using (var wrapMode = new ImageAttributes())
        //        {
        //            wrapMode.SetWrapMode(WrapMode.TileFlipXY);
        //            graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
        //        }
        //    }

        //    return destImage;
        //}

    }
}
