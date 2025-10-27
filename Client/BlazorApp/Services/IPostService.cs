using ApiContracts;

namespace BlazorApp.Services;

public interface IPostService
{
    Task<List<PostDTO>> GetAllPostsAsync(PostSearchFilter searchFilter);
    Task<PostDTO> GetPostByIdAsync(int id);
    Task<PostDTO> AddPostAsync(CreatePostDTO postDto);
}

public class PostSearchFilter
{
    public string? Title;
    public Int32? AuthorId;
    public Int32? CommentedOnPostId;
    public Int32? SubforumId;

    public static PostSearchFilter SearchForTitle(string title)
    {
        return new PostSearchFilter() { Title = title };
    }

    public static PostSearchFilter SearchForAuthorId(Int32 authorId)
    {
        return new PostSearchFilter() { AuthorId = authorId };
    }

    public static PostSearchFilter SearchForCommentedOnPostId(Int32 commentedOnPostId)
    {
        return new PostSearchFilter() { CommentedOnPostId = commentedOnPostId };
    }

    public static PostSearchFilter SearchForSubforumId(Int32 subforumId)
    {
        return new PostSearchFilter() { SubforumId = subforumId };
    }
}