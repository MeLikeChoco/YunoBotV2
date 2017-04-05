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

            string response = await CheckConnection(url);

            return JObject.Parse(response);

        }

        /// <summary>
        /// Will return null if service is down.
        /// </summary>
        /// <param name="url">The url to use</param>
        /// <returns>JArray</returns>
        public async Task<JArray> GetJArrayContent(string url)
        {

            string response = await CheckConnection(url);

            return JArray.Parse(response);

        }

        /// <summary>
        /// Will return null if service is down.
        /// </summary>
        /// <param name="url">The url to use</param>
        /// <returns>string</returns>
        public async Task<string> GetRawContent(string url)
        {
            return await CheckConnection(url);
        }

        /// <summary>
        /// Will return null if service is down.
        /// </summary>
        /// <param name="url">The url to use</param>
        /// <param name="type">The type to deserialize to</param>
        /// <returns></returns>
        public async Task<dynamic> GetDeserializedContent(string url, Type type)
        {

            string response = await CheckConnection(url);

            return JsonConvert.DeserializeObject(response, type);

        }

        private async Task<string> CheckConnection(string url)
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
