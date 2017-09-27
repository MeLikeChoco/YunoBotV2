using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YunoV3.Services;

namespace YunoV3.Modules.Commands.Pictures
{
    [RequireNsfw]
    public class Nsfw
    {

        private Web _web;

        public Nsfw(Web web)
            => _web = web;



    }
}
