using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace YunoBotV2.Services.WebServices
{
    public class WebService
    {

        public HttpClient Http { get; private set; }

        public WebService()
            => Http = new HttpClient();

        public async Task<JObject> GetJsonContent(string url)
        {

            int counter = 1;
            HttpResponseMessage response = await Http.GetAsync(url);

            while ((!response.IsSuccessStatusCode) && (counter < 4))
            {
                response = await Http.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
                counter++;
            }

            if (counter == 4) return null;

            return JObject.Parse(await response.Content.ReadAsStringAsync());

        }

        public async Task<string> GetRawContent(string url)
        {

            int counter = 1;
            HttpResponseMessage response = await Http.GetAsync(url);

            while ((!response.IsSuccessStatusCode) && (counter < 4))
            {
                response = await Http.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
                counter++;
            }

            if (counter == 4) return null;

            return await response.Content.ReadAsStringAsync();

        }

    }
}
