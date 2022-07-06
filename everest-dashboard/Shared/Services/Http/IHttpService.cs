using System;
namespace everest_dashboard.Shared.Services.Http
{
    public interface IHttpService
    {
        public abstract Task<HttpResponseWrapper<T>> GetAsync<T>(string endpoint);
        public abstract Task<HttpResponseWrapper<TResponse>> PostAsync<T, TResponse>(string endpoint, T requestObject);
        public abstract Task<HttpResponseWrapper<TResponse>> DeleteAsync<TResponse>(string endpoint);

    }
}

