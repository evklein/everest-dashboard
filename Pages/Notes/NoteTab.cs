using MudBlazor;

public class NoteTab
{
	public Note Note { get; set; }
	public Color IconColor {get; set;} = Color.Default;

	public NoteTab(Note note)
	{
		Note = note;
	}
}