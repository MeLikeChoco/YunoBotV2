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
using System.IO;

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

            if (string.IsNullOrEmpty(response))
                return null;

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

            if (string.IsNullOrEmpty(response))
                return null;

            try
            {
                return JArray.Parse(response);
            }
            catch { return new JArray(); }

        }

        /// <summary>
        /// Will return null if service is down or nothing is found
        /// </summary>
        /// <param name="url">The url to use</param>
        /// <returns>JObject</returns>
        public async Task<JToken> GetFirstJArrayContent(string url)
        {

            string response = await CheckConnection(url);

            if (string.IsNullOrEmpty(response))
                return null;

            try
            {
                return JArray.Parse(response).FirstOrDefault();
            }
            catch { return null; }

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
        /// <returns>dynamic</returns>
        public async Task<T> GetDeserializedContent<T>(string url)
        {

            string response = await CheckConnection(url);

            if (string.IsNullOrEmpty(response))
                return default(T);

            return JsonConvert.DeserializeObject<T>(response);

        }

        /// <summary>
        /// Will return null if service is down.
        /// </summary>
        /// <param name="url">The url to use</param>
        /// <param name="objectLocations">The path to the object location</param>
        /// <returns></returns>
        public async Task<T> GetDeserializedContent<T>(string url, params string[] objectLocations)
        {

            string response = await CheckConnection(url);

            if (string.IsNullOrEmpty(response))
                return default(T);

            dynamic jsonObject;

            try
            {
                jsonObject = JObject.Parse(response);

                foreach (var path in objectLocations)
                {

                    jsonObject = jsonObject[path];

                }
            }
            catch { return default(T); }

            return jsonObject.ToObject(typeof(T));

        }

        /// <summary>
        /// Will return null if service is down.
        /// </summary>
        /// <param name="url">The url to use</param>
        /// <returns>IHtmlDocument</returns>
        public async Task<IHtmlDocument> GetDom(string url)
        {

            string response = await CheckConnection(url);

            if (string.IsNullOrEmpty(response))
                return null;

            return await _parser.ParseAsync(response);

        }

        /// <summary>
        /// Returns true if post is successful and false if not
        /// </summary>
        /// <param name="url">The url to use</param>
        /// <param name="content">The content to post</param>
        /// <returns>bool</returns>
        public async Task<bool> Post(string url, HttpContent content)
        {

            int counter = 0;
            HttpResponseMessage response;

            do
            {

                response = await _http.PostAsync(url, content);
                counter++;

            } while ((!response.IsSuccessStatusCode) && (counter < 3));

            if (counter == 3) return false;
            else return true;

        }

        /// <summary>
        /// Returns true if post is successful and false if not
        /// </summary>
        /// <param name="url">The url to use</param>
        /// <param name="content">The content to post</param>
        /// <param name="result">The content after posting</param>
        /// <returns>bool</returns>
        public Task<bool> Post(string url, HttpContent content, out string result)
        {

            int counter = 0;
            HttpResponseMessage response;

            do
            {

                response = _http.PostAsync(url, content).Result;
                counter++;

            } while ((!response.IsSuccessStatusCode) && (counter < 3));

            if (counter == 3)
            {
                result = null;
                return Task.FromResult(false);
            }
            else
            {
                result = response.Content.ReadAsStringAsync().Result;
                return Task.FromResult(true);
            }

        }

        /// <summary>
        /// Return the content in the form of a stream
        /// </summary>
        /// <param name="url">The url to use</param>
        /// <returns>Stream</returns>
        public async Task<Stream> GetStream(string url)
        {

            int counter = 0;
            HttpResponseMessage response;

            do
            {

                response = await _http.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
                counter++;

            } while ((!response.IsSuccessStatusCode) && (counter < 3));

            if (counter == 3) return null;

            return await response.Content.ReadAsStreamAsync();

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
