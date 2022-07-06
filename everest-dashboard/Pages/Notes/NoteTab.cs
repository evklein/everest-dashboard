using everest_common.Enumerations;
using everest_common.Models;
using MudBlazor;

namespace everest_dashboard.Pages.Notes;

public class NoteTab
{
	public Note Note { get; set; }
	public bool TitleIsEditable { get; set; } = false;
	public NoteViewEnum NoteView { get; set; } = NoteViewEnum.Outline;

	public NoteTab(Note note)
	{
		Note = note;
	}
}