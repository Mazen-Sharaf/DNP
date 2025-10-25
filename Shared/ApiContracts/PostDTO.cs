namespace ApiContracts;

public class PostDTO
{
    public int PostId { get; set; }
    public string? Title { get; set; } 
    public string Content { get; set; }
    public int AuthorId { get; set; }
    public int SubforumId { get; set; }
    public int? CommentedOnPostId { get; set; }
    
}