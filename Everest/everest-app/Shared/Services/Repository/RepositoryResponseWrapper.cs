using System;
namespace everest_app.Shared.Services.Repository
{
    public class RepositoryResponseWrapper<TResponseObject>
    {
        public bool Success { get; set; } = true;
        public TResponseObject? Value { get; set; }
        public RepositoryResponseError? Error { get; set; }
    }
}

