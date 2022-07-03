using System;
namespace everest_dashboard.Shared.Services
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient _httpClient;

        public HttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> DoNetworkTest()
        {
            return await _httpClient.GetStringAsync("/WeatherForecast");
        }
    }
}

