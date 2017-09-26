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
using YunoV3.Services;

namespace YunoV3.Modules.Commands
{
    [RequireContext(ContextType.Guild)]
    public class Battle : CustomBase
    {

        private Web _web;

        public Battle(Web web)
            => _web = web;

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
                    font = new Font(SystemFonts.Find("Segoe UI"), 30);
                else
                    font = new Font(SystemFonts.Find("DejaVu Sans"), 25);

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
                    processor.DrawText(user.Nickname ?? user.Username, font, Rgba32.Green, new Vector2(195, 606), textOptions);
                    processor.DrawText(enemy.Nickname ?? enemy.Username, font, Rgba32.Red, new Vector2(700, 606), textOptions);

                });

                baseImage.SaveAsPng(output, pngEncoder);
                output.Seek(0, SeekOrigin.Begin);

                await UploadAsync(output, "deathbattle.png");

            }

        }

        private async Task<Stream> GetAvatarStream(SocketGuildUser user)
        {

            if (user.GetAvatarUrl() != null)
                return (await _web.GetStreamAsync(user.GetAvatarUrl())).stream;
            else
            {

                var filepath = Directory.GetFiles("Files/Images/Default Discord Avatars/").RandomSubset(1).First();
                return File.Open(filepath, FileMode.Open);

            }

        }

    }
}
