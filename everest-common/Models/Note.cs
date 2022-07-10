
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace everest_common.Models;

public class Note
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string Title { get; set; } = string.Empty;

    [NotMapped]
    public string UpdatedTitle { get; set; }

    public string Content { get; set; } = string.Empty;

    [NotMapped]
    public string UpdatedContent { get; set; } = string.Empty;

    public DateTime LastModified { get; set; }

    public Note() : this(string.Empty, string.Empty) { }

    public string OwnerId { get; set; }
    public IdentityUser Owner { get; set; }

    public Note(string title, string content)
    {
        Title = title;
        UpdatedTitle = title;

        Content = content;
        UpdatedContent = content;
    }

    [NotMapped]
    public bool NoteHasChanged => !Title.Equals(UpdatedTitle) || !Content.Equals(UpdatedContent);

    public void ResetToLastSavedState()
    {
        UpdatedTitle = Title;
        UpdatedContent = Content;
    }
}