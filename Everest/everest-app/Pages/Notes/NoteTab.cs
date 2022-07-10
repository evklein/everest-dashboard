using everest_common.Enumerations;
using everest_common.Models;

namespace everest_app.Pages.Notes;

public class NoteTab
{
	public Note Note { get; set; }
	public NoteViewEnum NoteView { get; set; } = NoteViewEnum.Outline;

	public NoteTab(Note note)
	{
		Note = note;
	}
}