namespace ApiContracts;

public class CreatePostDTO
{
    public string? Title { get; set; }
    public string content { get; set; }
    public int AuthorId { get; set; }
    public int SubforumId { get; set; }
    public int? CommentedOnPostId { get; set; }
}