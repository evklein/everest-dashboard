using System;
using everest_common.Models;

namespace everest_common.DataTransferObjects.Notes
{
    public class SaveNoteResponseDataTransferObject
    {
        public List<NoteListItem> NoteListItems { get; set; }
        public Note SavedNote { get; set; }
    }
}

