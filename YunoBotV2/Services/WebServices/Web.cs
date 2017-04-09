using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using AngleSharp.Parser.Html;
using AngleSharp.Dom.Html;

namespace YunoBotV2.Services.WebServices
{
    public class Web
    {

        public HttpClient _http;
        private HtmlParser _parser;

        public Web()
        {

            _http = new HttpClient();
            _parser = new HtmlParser();

        }

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
        /// Will return null if service is down or nothing is found
        /// </summary>
        /// <param name="url">The url to use</param>
        /// <returns>JObject</returns>
        public async Task<JToken> GetFirstJArrayContent(string url)
        {

            string response = await CheckConnection(url);
            
            return JArray.Parse(response).FirstOrDefault();

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
        /// <returns>dynamic</returns>
        public async Task<dynamic> GetDeserializedContent(string url, Type type)
        {

            string response = await CheckConnection(url);

            return JsonConvert.DeserializeObject(response, type);

        }

        /// <summary>
        /// Will return null if service is down.
        /// </summary>
        /// <param name="url">The url to use</param>
        /// <param name="type">The type to deserialize to at the very end</param>
        /// <param name="objectLocations">The path to the object location</param>
        /// <returns></returns>
        public async Task<dynamic> GetDeserializedContent(string url, Type type, params string[] objectLocations)
        {

            string response = await CheckConnection(url);
            dynamic jsonObject = JObject.Parse(response);

            foreach(var path in objectLocations)
            {

                jsonObject = jsonObject[path];

            }

            return jsonObject.ToObject(type);

        }

        /// <summary>
        /// Will return null if service is down.
        /// </summary>
        /// <param name="url">The url to use</param>
        /// <returns>IHtmlDocument</returns>
        public async Task<IHtmlDocument> GetDom(string url)
        {

            string response = await CheckConnection(url);

            return await _parser.ParseAsync(response);

        }

        private async Task<string> CheckConnection(string url)
        {

            int counter = 0;
            HttpResponseMessage response;

            do
            {

                response = await _http.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
                counter++;

            } while ((!response.IsSuccessStatusCode) && (counter < 3));

            if (counter == 3) return null;

            return await response.Content.ReadAsStringAsync();

        }

    }
}
