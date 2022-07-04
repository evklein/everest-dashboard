using System.ComponentModel.DataAnnotations.Schema;

namespace everest_common.Models;

public class Note
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Title { get; set; } = string.Empty;

    [NotMapped]
    public string UpdatedTitle { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    [NotMapped]
    public string UpdatedContent { get; set; } = string.Empty;

    public DateTime LastModified { get; set; }

    public Note() : this(string.Empty, string.Empty) { }

    public Note(string title, string content)
    {
        Title = title;
        UpdatedTitle = title;

        Content = content;
        UpdatedContent = content;
    }

    public bool NoteHasChanged => !Title.Equals(UpdatedTitle) || !Content.Equals(UpdatedContent);
}