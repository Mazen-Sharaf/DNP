using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class ReactionFileRepository : IReactionRepository
{
    private readonly string _filePath = "reactions.json";

    public ReactionFileRepository()
    {
        if (!File.Exists(_filePath))
        {
            File.WriteAllText(_filePath, "[]");
        }
    }

    private async Task<List<Reaction>> LoadListFromFileAsync()
    {
        string json = await File.ReadAllTextAsync(_filePath);
        List<Reaction> reactions = JsonSerializer.Deserialize<List<Reaction>>(json)!;
        return reactions;
    }

    private async Task UpdateFileAsync(List<Reaction> reactions)
    {
        string reactionsAsJson = JsonSerializer.Serialize(reactions);
        await File.WriteAllTextAsync(_filePath, reactionsAsJson);
    }
    
    public async Task<Reaction> AddAsync(Reaction reaction)
    {
        List<Reaction> reactions = await LoadListFromFileAsync();
        
        if (reactions.Any(r => r.PostId == reaction.PostId && r.ByUserId == reaction.ByUserId && r.Type == reaction.Type))
        {
            throw new InvalidOperationException("Reaction already exists");
        }
        reactions.Add(reaction);
        
        await UpdateFileAsync(reactions);
        
        return reaction;
    }

    public async Task DeleteAsync(Reaction reaction)
    {
        List<Reaction> reactions = await LoadListFromFileAsync();
        
        Reaction? reactionToRemove = reactions.SingleOrDefault(r => r.PostId == reaction.PostId && r.ByUserId == reaction.ByUserId && r.Type == reaction.Type);
        
        if (reactionToRemove == null) throw new InvalidOperationException("Reaction not found");
        
        reactions.Remove(reactionToRemove);
        
        await UpdateFileAsync(reactions);
    }

    public async Task DeleteAllAsync(Post post)
    {
        List<Reaction> reactions = await LoadListFromFileAsync();
        
        reactions.RemoveAll(r => r.PostId == post.PostId);
        
        await UpdateFileAsync(reactions);
    }

    public async Task DeleteAllAsync(User user)
    {
        List<Reaction> reactions = await LoadListFromFileAsync();
        
        reactions.RemoveAll(r => r.ByUserId == user.UserId);
        
        await UpdateFileAsync(reactions);
    }

    public async Task<int> GetTotalOfTypeAsync(Post post, string type)
    {
        List<Reaction> reactions = await LoadListFromFileAsync();
        return reactions.Count(r => r.PostId == post.PostId && r.Type == type);
    }

    public IQueryable<Reaction> GetMany()
    {
        List<Reaction> reactions = LoadListFromFileAsync().Result;
        return reactions.AsQueryable();
    }
}