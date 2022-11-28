using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Web;
using System.Collections.Specialized;
using PS.Connector.Base.Interface;

namespace PS.Base.Connector
{
    public class BaseApiClient : IBaseApiClient
    {
        public HttpClient _httpClient;

        public async Task<HttpResponseMessage> GetAsync(string endpoint, Dictionary<string, string> @params = null)
        {
            try
            {
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, endpoint);
                NameValueCollection query = HttpUtility.ParseQueryString("");

                if (@params != null)
                {
                    foreach (var param in @params)
                    {
                        query[param.Key] = param.Value;
                    }
                }

                var builder = new UriBuilder(new Uri(string.Concat(_httpClient.BaseAddress, endpoint)));
                builder.Query = query.ToString();
                
                return await _httpClient.GetAsync(builder.Uri);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<HttpResponseMessage> PostAsync(object body, string endpoint)
        {
            try
            {
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, string.Concat(_httpClient.BaseAddress, endpoint));

                message.Content = new StringContent(JsonSerializer.Serialize(body));
                message.Headers.Add("Content-Type", "application/json");

                return await _httpClient.SendAsync(message);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        protected void SetHttpClient(HttpClient client) => _httpClient = client;
    }
}
