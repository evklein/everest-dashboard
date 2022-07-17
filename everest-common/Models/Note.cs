
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
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

    public virtual ICollection<Tag> Tags { get; set; }

    [NotMapped]
    public virtual ICollection<Tag> UpdatedTags { get; set; } = new List<Tag>();

    public Note(string title, string content)
    {
        Title = title;
        UpdatedTitle = title;

        Content = content;
        UpdatedContent = content;
    }

    public bool NoteHasChanged()
    {
        var titleEquals = Title.Equals(UpdatedTitle);
        var contentEquals = Content.Equals(UpdatedContent);

        var currentTagsCount = Tags?.Count ?? 0;
        var updatedTagsCount = UpdatedTags?.Count ?? 0;
        var tagsEquals = currentTagsCount == updatedTagsCount;
        foreach (var tag in Tags) // TODO: Improve so that it doesn't require a code update if a property changes.
        {
            var matchingTag = UpdatedTags.Where(t => t.ColorHexadecimal == tag.ColorHexadecimal)
                                         .Where(t => t.DateCreated.Equals(tag.DateCreated))
                                         .Where(t => t.Id == tag.Id)
                                         .Where(t => t.Name.Equals(tag.Name))
                                         .Where(t => t.OwnerId == tag.OwnerId)
                                         .SingleOrDefault();
            if (matchingTag is null)
            {
                tagsEquals = false;
                break;
            }
        }

        return !titleEquals || !contentEquals || !tagsEquals;
    }

    public void ResetToLastSavedState()
    {
        UpdatedTitle = Title;
        UpdatedContent = Content;
    }
}