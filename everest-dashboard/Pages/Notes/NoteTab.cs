using MudBlazor;

public class NoteTab
{
	public Note Note { get; set; }

	public bool TitleIsEditable { get; set; } = false;

	public NoteTab(Note note)
	{
		Note = note;
	}
}