using System;
namespace everest_dashboard.Shared.Services
{
    public interface IHttpService
    {
        public abstract Task<string> DoNetworkTest();
    }
}

