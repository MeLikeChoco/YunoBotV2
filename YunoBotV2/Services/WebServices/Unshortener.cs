using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace YunoBotV2.Services.WebServices
{
    public class Unshortener : WebService
    {
        
        public Unshortener()
        {
            var handler = new HttpClientHandler
            {
                AllowAutoRedirect = false,
            };

            _http = new HttpClient(handler);
        }

        public async Task<string> Get(string url)
        {

            HttpResponseMessage response = await _http.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            return response.Headers.Location.AbsoluteUri;

        }

    }
}
