using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YunoBotV2.Services.WebServices;

namespace YunoBotV2.Commands
{

    [Group("mc")]
    public class Minecraft : CustomModuleBase
    {

        private const char Ampersand = '§';
        private const string MCAPI = "https://mcapi.ca";
        private const string FileName = "minecraftskin.png";
        private Web _web;

        public Minecraft(Web web)
        {

            _web = web;

        }
        
        [Command("avatar")]
        [Summary("Get a player's avatar")]
        public async Task McAvatarCommand([Remainder]string name)
        {

            var url = MCAPI + $"/avatar/{Uri.EscapeUriString(name)}";
            var stream = await _web.GetStream(url);

            if (stream == null)
            {

                await NoResultsReturnedErrorMessage();
                return;

            }

            await UploadAsync(stream, FileName);

        }

        [Command("skin")]
        [Summary("Get a 2D view of a skin")]
        public async Task McSkinCommand([Remainder]string name)
        {

            var url = MCAPI + $"/skin/{Uri.EscapeUriString(name)}";
            var stream = await _web.GetStream(url);

            if(stream == null)
            {

                await NoResultsReturnedErrorMessage();
                return;

            }

            await UploadAsync(stream, FileName);

        }

        [Command("rawskin")]
        [Summary("Get the file of a skin")]
        public async Task McRawSkinCommand([Remainder]string name)
        {

            var url = MCAPI + $"/rawskin/{Uri.EscapeUriString(name)}";
            var stream = await _web.GetStream(url);

            if (stream == null)
            {

                await NoResultsReturnedErrorMessage();
                return;

            }

            await UploadAsync(stream, FileName);

        }

        [Group("render")]
        public class MinecraftRenders : CustomModuleBase
        {

            private const char Ampersand = '§';
            private const string Crafatar = "https://crafatar.com/renders";
            private const string FileExtension = "png";
            private Web _web;

            public MinecraftRenders(Web web)
            {

                _web = web;

            }

            [Command("head")]
            [Summary("Render the head of a skin")]
            public async Task RenderHeadCommand([Remainder]string name)
            {

                var url = Crafatar + $"/head/{Uri.EscapeUriString(name)}?scale=10";
                var stream = await _web.GetStream(url);

                if (stream == null)
                {

                    await NoResultsReturnedErrorMessage();
                    return;

                }

                await UploadAsync(stream, FileExtension);

            }

            [Command("body")]
            [Summary("Render the body of a skin")]
            public async Task RenderBodyCommand([Remainder]string name)
            {

                var url = Crafatar + $"/body/{Uri.EscapeUriString(name)}?scale=10";
                var stream = await _web.GetStream(url);

                if (stream == null)
                {

                    await NoResultsReturnedErrorMessage();
                    return;

                }

                await UploadAsync(stream, FileExtension);

            }

        }

    }
}
