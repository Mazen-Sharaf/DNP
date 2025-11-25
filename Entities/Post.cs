namespace Entities;

public class Post
{
    public int PostId { get; set; }
    public string? Title { get; set; }
    public string Content { get; set; }
    public User Author { get; set; }
    public Subforum InSubforum { get; set; }

    public Post?
        CommentedOnPostId { get; set; }
}