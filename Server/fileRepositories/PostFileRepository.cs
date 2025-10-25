using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class PostFileRepository : IPostRepository
{
    private readonly string _filePath = "posts.json";

    public PostFileRepository()
    {
        if (!File.Exists(_filePath))
        {
            File.WriteAllText(_filePath, "[]");
        }
    }

    private async Task<List<Post>> LoadListFromFileAsync()
    {
        string json = await File.ReadAllTextAsync(_filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(json)!;
        return posts;
    }

    private async Task UpdateFileAsync(List<Post> posts)
    {
        string postsAsJson = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(_filePath, postsAsJson);
    }
    
    public async Task<Post> AddAsync(Post post)
    {
        List<Post> posts = await LoadListFromFileAsync();
        
        post.PostId = posts.Count != 0 ? posts.Max(p => p.PostId) + 1 : 1;
        posts.Add(post);
        
        await UpdateFileAsync(posts);
        
        return post;
    }

    public async Task UpdateAsync(Post post)
    {
        List<Post> posts = await LoadListFromFileAsync();
        
        Post? existingPost = posts.SingleOrDefault(p => p.PostId == post.PostId);
        if (existingPost is null) throw new InvalidOperationException($"Post with ID '{post.PostId}' not found");

        posts.Remove(existingPost);
        posts.Add(post);

        await UpdateFileAsync(posts);
    }

    public async Task DeleteAsync(int id)
    {
        List<Post> posts = await LoadListFromFileAsync();
        
        Post? postToRemove = posts.SingleOrDefault(p => p.PostId == id);
        if (postToRemove is null) throw new InvalidOperationException ($"Post with ID '{id}' not found");

        posts.Remove(postToRemove);
        
        // Fjern alle kommentarer
        var comments = posts.FindAll(p => p.CommentedOnPostId == id);
        foreach (var comment in comments) await DeleteAsync(comment.PostId);
        
        await UpdateFileAsync(posts);
    }

    public async Task DeleteAllFromSubforumAsync(int subforumId)
    {
        List<Post> posts = await LoadListFromFileAsync();
        
        foreach (var post in posts)
        {
            if (post.PostId == subforumId) await DeleteAsync(post.PostId);
        }
        
        await UpdateFileAsync(posts);
    }

    public async Task DeleteAllFromUserAsync(int userId)
    {
        List<Post> posts = await LoadListFromFileAsync();
        
        foreach (var post in posts)
        {
            if (post.AuthorId == userId) await DeleteAsync(post.PostId);
        }
        
        await UpdateFileAsync(posts);
    }

    public async Task<Post> GetSingleAsync(int id)
    {
        List<Post> posts = await LoadListFromFileAsync();
        
        Post? post = posts.SingleOrDefault(p => p.PostId == id);
        if (post is null) throw new InvalidOperationException($"Post with ID '{id}' not found");
        
        await UpdateFileAsync(posts);
        
        return post;
    }

    public IQueryable<Post> GetMany()
    {
        var posts = LoadListFromFileAsync().Result;
        return posts.AsQueryable();
    }
}