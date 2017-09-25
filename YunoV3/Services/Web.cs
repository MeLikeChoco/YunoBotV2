using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using YunoV3.Objects.Exceptions;

namespace YunoV3.Services
{
    public class Web
    {

        private HttpClient _client;
        private HtmlParser _htmlParser;

        public Web()
        {

            _client = new HttpClient();
            _htmlParser = new HtmlParser();

        }

        public Task<HttpResponseMessage> PostAsync(string url, string content, string auth)
            => PostAsync(url, content, auth: auth);

        public Task<HttpResponseMessage> PostAsync(string url, string content, string contentType = null, string auth = null)
        {

            var payload = new HttpRequestMessage(HttpMethod.Post, url)
            {

                Content = new StringContent(content)

            };

            if (!string.IsNullOrEmpty(auth))
                payload.Headers.Authorization = new AuthenticationHeaderValue(auth);

            if (!string.IsNullOrEmpty(contentType))
                payload.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);

            return _client.SendAsync(payload, HttpCompletionOption.ResponseHeadersRead);

        }

        public async Task<JObject> GetJObjectAsync(string url)
        {

            var content = await CheckUrl(url);

            return JObject.Parse(await content.ReadAsStringAsync());

        }

        public async Task<JArray> GetJArrayAsync(string url)
        {

            var content = await CheckUrl(url);

            return JArray.Parse(await content.ReadAsStringAsync());

        }
 
        public async Task<IDocument> GetDomAsync(string url)
        {

            var content = await CheckUrl(url);

            return await _htmlParser.ParseAsync(await content.ReadAsStringAsync());

        }

        public async Task<T> GetDeserializedObjectAsync<T>(string url)
        {

            var content = await CheckUrl(url);

            return JsonConvert.DeserializeObject<T>(await content.ReadAsStringAsync());

        }

        public async Task<string> GetStringAsync(string url)
        {

            var content = await CheckUrl(url);

            return await content.ReadAsStringAsync();

        }

        public async Task<(Stream stream, string filename)> GetStreamAsync(string url)
        {

            var content = await CheckUrl(url);
            var stream = await content.ReadAsStreamAsync();
            var copy = new MemoryStream();

            await stream.CopyToAsync(copy);
            copy.Seek(0, SeekOrigin.Begin);

            var filename = content.Headers.ContentDisposition?.FileName;

            return (copy, filename);

        }

        private async Task<HttpContent> CheckUrl(string url)
        {

            HttpResponseMessage response;
            var counter = 0;

            do
            {

                try
                {

                    response = await _client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
                    counter++;

                }
                catch
                {

                    throw new WebServiceException();

                }

            } while (!response.IsSuccessStatusCode && counter != 3);

            return response.Content;

        }

    }
}
