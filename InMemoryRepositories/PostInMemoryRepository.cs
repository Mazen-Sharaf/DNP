using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class PostInMemoryRepository : IPostRepository
{
    private List<Post> posts = [];

    public Task<Post> AddAsync(Post post)
    {
        post.PostId = posts.Count != 0 ? posts.Max(p => p.PostId) + 1 : 1;
        posts.Add(post);
        return Task.FromResult(post);
    }

    public Task UpdateAsync(Post post)
    {
        Post? existingPost = posts.SingleOrDefault(p => p.PostId == post.PostId);
        if (existingPost is null) throw new InvalidOperationException($"Post with ID '{post.PostId}' not found");

        posts.Remove(existingPost);
        posts.Add(post);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        Post? postToRemove = posts.SingleOrDefault(p => p.PostId == id);
        if (postToRemove is null) throw new InvalidOperationException($"Post with ID '{id}' not found");

        posts.Remove(postToRemove);

        // Fjern alle kommentarer
        var comments = posts.FindAll(p => p.CommentedOnPostId == id);
        foreach (var comment in comments) DeleteAsync(comment.PostId);

        return Task.CompletedTask;
    }

    public Task DeleteAllFromSubforumAsync(int subforumId)
    {
        foreach (var post in posts)
        {
            if (post.PostId == subforumId) DeleteAsync(post.PostId);
        }

        return Task.CompletedTask;
    }

    public Task DeleteAllFromUserAsync(int userId)
    {
        foreach (var post in posts)
        {
            if (post.AuthorId == userId) DeleteAsync(post.PostId);
        }

        return Task.CompletedTask;
    }

    public Task<Post> GetSingleAsync(int id)
    {
        Post? post = posts.SingleOrDefault(p => p.PostId == id);
        if (post is null) throw new InvalidOperationException($"Post with ID '{id}' not found");

        return Task.FromResult(post);
    }

    public IQueryable<Post> GetMany()
    {
        return posts.AsQueryable();
    }
}