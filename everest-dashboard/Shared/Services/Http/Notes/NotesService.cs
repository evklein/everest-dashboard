using System;
using everest_common.Models;

namespace everest_dashboard.Shared.Services.Http.Notes
{
    public class NotesService : INotesService
    {
        private readonly IHttpService _httpService;
        private readonly string _endpoint = "api/notes";

        public NotesService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<IEnumerable<Note>> GetNotesAsync()
        {
            var getNotesResponse = await _httpService.GetAsync<IEnumerable<Note>>(_endpoint);
            if (getNotesResponse.Success)
            {
                return getNotesResponse.ResponseObject;
            }

            return new List<Note>();
        }

        public async Task<IEnumerable<Note>> SaveNoteAsync(Note note)
        {
            var saveNoteResponse = await _httpService.PostAsync<Note, IEnumerable<Note>>(_endpoint, note);
            if (saveNoteResponse.Success)
            {
                return saveNoteResponse.ResponseObject;
            }

            return new List<Note>();
        }
    }
}

