using System;
using System.ComponentModel.Design;
using System.Text;
using Newtonsoft.Json;

namespace everest_app.Shared.Services.Http
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient _httpClient;

        public HttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseWrapper<T>> GetAsync<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {
                var responseObject = await deserializeHttpResponse<T>(response);
                return new HttpResponseWrapper<T>(responseObject, true, response);
            }
            else
            {
                return new HttpResponseWrapper<T>(default, false, response);
            }
        }

        public async Task<HttpResponseWrapper<TResponse>> PostAsync<T, TResponse>(string endpoint, T requestObject)
        {
            var json = JsonConvert.SerializeObject(requestObject);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(endpoint, stringContent);

            if (response.IsSuccessStatusCode)
            {
                var responseObject = await deserializeHttpResponse<TResponse>(response);
                return new HttpResponseWrapper<TResponse>(responseObject, true, response);
            }
            else
            {
                return new HttpResponseWrapper<TResponse>(default, false, response);
            }
        }

        public async Task<HttpResponseWrapper<TResponse>> DeleteAsync<TResponse>(string endpoint)
        {
            var response = await _httpClient.DeleteAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {
                var responseObject = await deserializeHttpResponse<TResponse>(response);
                return new HttpResponseWrapper<TResponse>(responseObject, true, response);
            }
            else
            {
                return new HttpResponseWrapper<TResponse>(default, false, response);
            }
        }

        private async Task<T> deserializeHttpResponse<T>(HttpResponseMessage httpResponseMessage)
        {
            var httpResponseString = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(httpResponseString);
        }
    }
}

