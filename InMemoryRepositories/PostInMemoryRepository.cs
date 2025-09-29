using Entities;
using RepositoryContracts;

namespace InMemoryRepositories; 

public class PostInMemoryRepository: IPostRepository
{
    private List<Post> _posts =[];
    public Task<Post> AddAsync(Post post)
    {
        post.PostId = _posts.Any()
            ? _posts.Max(p => p.PostId) + 1
            : 1;
        _posts.Add(post);
        return Task.FromResult(post);
    }
    public Task UpdateAsync(Post post)
    {
        Post? existingPost = _posts.SingleOrDefault(p => p.PostId == post.PostId);
        if (existingPost is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{post.PostId}' not found");
        }

        _posts.Remove(existingPost);
        _posts.Add(post);

        return Task.CompletedTask;
    }
    public Task DeleteAsync(int id)
    {
        Post? postToRemove = _posts.SingleOrDefault(p => p.PostId == id);
        if (postToRemove is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{id}' not found");
        }

        _posts.Remove(postToRemove);
        return Task.CompletedTask;
    }

    public Task DeleteAllFromSubforumAsync(int subforumId)
    {
        foreach (var post in _posts)
        {
            if (post.PostId == subforumId) DeleteAsync(post.PostId);
        }
        
        return Task.CompletedTask;
    }

    public Task DeleteAllFromUserAsync(int userId)
    {
        foreach (var post in _posts)
        {
            if (post.AuthorId == userId) DeleteAsync(post.PostId);
        }
        
        return Task.CompletedTask;
    }

    public Task<Post> GetSingleAsync(int id)
    {
        Post? postToReturn = _posts.SingleOrDefault(p => p.PostId == id);
        if (postToReturn is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{id}' not found");
        }

        return Task.FromResult(postToReturn);
    } 
    public IQueryable<Post> GetMany()
    {
        return _posts.AsQueryable();
    }
}