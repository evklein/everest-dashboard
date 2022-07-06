using System;
namespace everest_dashboard.Shared.Services.Http
{
    public class HttpResponseWrapper<T>
    {
        public T ResponseObject { get; set; }
        public bool Success { get; set; }
        public HttpResponseMessage HttpResponseMessage { get; set; }

        public HttpResponseWrapper(T responseObject, bool success, HttpResponseMessage httpResponseMessage)
        {
            ResponseObject = responseObject;
            Success = success;
            HttpResponseMessage = httpResponseMessage;
        }

        public async Task<string> GetBody()
        {
            return await HttpResponseMessage.Content.ReadAsStringAsync();
        }
    }
}

