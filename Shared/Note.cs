public class Note
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;

    public string UpdatedContent { get; set; } = string.Empty;
    public DateTime LastModified { get; set; }

    public Note() : this(string.Empty, string.Empty) { }

    public Note(string title, string content)
    {
        Title = title;
        Content = content;
        UpdatedContent = content;
    }

    public bool ContentHasChanged => !Content.Equals(UpdatedContent);
}