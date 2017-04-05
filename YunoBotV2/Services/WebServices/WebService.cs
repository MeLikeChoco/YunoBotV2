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

        protected HttpClient _http;

        public WebService()
            => _http = new HttpClient();

        /// <summary>
        /// Will return null if service is down.
        /// </summary>
        /// <param name="url">The url to use</param>
        /// <returns>JObject</returns>
        public async Task<JObject> GetJObjectContent(string url)
        {

            int counter = 1;
            HttpResponseMessage response = await _http.GetAsync(url);

            while ((!response.IsSuccessStatusCode) && (counter < 4))
            {
                response = await _http.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
                counter++;
            }

            if (counter == 4) return null;

            return JObject.Parse(await response.Content.ReadAsStringAsync());

        }

        /// <summary>
        /// Will return null if service is down.
        /// </summary>
        /// <param name="url">The url to use</param>
        /// <returns>JArray</returns>
        public async Task<JArray> GetJArrayContent(string url)
        {

            int counter = 1;
            HttpResponseMessage response = await _http.GetAsync(url);

            while ((!response.IsSuccessStatusCode) && (counter < 4))
            {
                response = await _http.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
                counter++;
            }

            if (counter == 4) return null;

            return JArray.Parse(await response.Content.ReadAsStringAsync());

        }

        /// <summary>
        /// Will return null if service is down.
        /// </summary>
        /// <param name="url">The url to use</param>
        /// <returns>string</returns>
        public async Task<string> GetRawContent(string url)
        {

            int counter = 1;
            HttpResponseMessage response = await _http.GetAsync(url);

            while ((!response.IsSuccessStatusCode) && (counter < 4))
            {
                response = await _http.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
                counter++;
            }

            if (counter == 4) return null;

            return await response.Content.ReadAsStringAsync();

        }

    }
}
