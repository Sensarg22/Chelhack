using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Common
{
    public class JsonStreamHttpClient
    {
        private readonly HttpClient _httpClient;

        public JsonStreamHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public async Task<T> GetFromJsonStreamAsync<T>(string url)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                using (var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                {
                    var stream = await response.Content.ReadAsStreamAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        return DeserializeJsonFromStream<T>(stream);
                    }

                    var content = await StreamToStringAsync(stream);
                    throw new Exception(content);
                }
            }
        }
        
        private static T DeserializeJsonFromStream<T>(Stream stream)
        {
            if (stream == null || stream.CanRead == false)
            {
                return default(T);
            }

            using (var streamReader = new StreamReader(stream))
            {
                using (var textReader = new JsonTextReader(streamReader))
                {
                    var serializer = new JsonSerializer();
                    var searchResult = serializer.Deserialize<T>(textReader);
                    return searchResult;
                }
            }
        }
        
        private static async Task<string> StreamToStringAsync(Stream stream)
        {
            if (stream == null)
            {
                return null;
            }
            
            using (var sr = new StreamReader(stream))
            {
                return await sr.ReadToEndAsync();
            }
        }
    }
}