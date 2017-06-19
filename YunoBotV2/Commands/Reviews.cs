using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YunoBotV2.Configuration;
using YunoBotV2.Services.WebServices;

namespace YunoBotV2.Commands
{
    [Group("reviews")]
    public class Reviews : CustomModuleBase
    {

        private Web _service;

        public Reviews(Web webParams)
        {

            _service = webParams;

        }

        [Command]
        public async Task ReviewsSearchCommand([Remainder]string search)
        {



        }

    }
}
