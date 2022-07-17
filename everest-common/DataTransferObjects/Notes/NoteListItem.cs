using System;
using everest_common.Models;

namespace everest_common.DataTransferObjects.Notes
{
    public class NoteListItem
    {
        public Guid NoteId { get; set; }
        public string NoteTitle { get; set; }
        public List<Tag> NoteTags { get; set; }
        public DateTime NoteDateModified { get; set; }

        public int NumberOfTagsToShow => NoteTags.Count >= 3 ? 3 : NoteTags.Count;
    }
}

