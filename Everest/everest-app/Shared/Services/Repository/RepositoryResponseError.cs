using System;
namespace everest_app.Shared.Services.Repository
{
    public class RepositoryResponseError
    {
        public string ErrorMessage { get; set; }
        public Exception? InnerException { get; set; }
    }
}

