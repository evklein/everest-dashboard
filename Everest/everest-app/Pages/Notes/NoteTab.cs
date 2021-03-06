using everest_common.Enumerations;
using everest_common.Models;

namespace everest_app.Pages.Notes;

public class NoteTab
{
	public Note Note { get; set; }
	public NoteViewEnum NoteView { get; set; } = NoteViewEnum.Split;
	public bool Loading { get; set; } = false;

	public NoteTab(Note note)
	{
		Note = note;
	}
}