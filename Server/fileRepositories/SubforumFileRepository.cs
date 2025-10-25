using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class SubforumFileRepository : ISubforumRepository
{
    private readonly string _filePath = "subforums.json";

    public SubforumFileRepository()
    {
        if (!File.Exists(_filePath))
        {
            File.WriteAllText(_filePath, "[]");
        }
    }

    private async Task<List<Subforum>> LoadListFromFileAsync()
    {
        string json = await File.ReadAllTextAsync(_filePath);
        List<Subforum> subforums = JsonSerializer.Deserialize<List<Subforum>>(json)!;
        return subforums;
    }

    private async Task UpdateFileAsync(List<Subforum> subforums)
    {
        string subforumsAsJson = JsonSerializer.Serialize(subforums);
        await File.WriteAllTextAsync(_filePath, subforumsAsJson);
    }
    public async Task<Subforum> AddAsync(Subforum subforum)
    {  
        List<Subforum> subforums = await LoadListFromFileAsync();
        
        subforum.SubforumId = subforums.Count != 0 ? subforums.Max(sf => sf.SubforumId) + 1 : 1;
        subforums.Add(subforum);
        
        await UpdateFileAsync(subforums);
        
        return subforum;
    }

    public async Task UpdateAsync(Subforum subforum)
    {
        List<Subforum> subforums = await LoadListFromFileAsync();
        
        Subforum? existingSubforum = subforums.SingleOrDefault(sf => sf.SubforumId == subforum.SubforumId);
        if (existingSubforum is null) throw new InvalidOperationException($"post with ID '{subforum.SubforumId}' not found");

        subforums.Remove(existingSubforum);
        subforums.Add(subforum);
        
        await UpdateFileAsync(subforums);
    }

    public async Task DeleteAsync(int id)
    {
        List<Subforum> subforums = await LoadListFromFileAsync();
        
        Subforum? subforumToRemove = subforums.SingleOrDefault(sf => sf.SubforumId == id);
        if (subforumToRemove is null) throw new InvalidOperationException ($"Post with ID '{id}' not found");

        subforums.Remove(subforumToRemove);
        
        await UpdateFileAsync(subforums);
    }

    public async Task<Subforum> GetSingleAsync(int id)
    {
        List<Subforum> subforums = await LoadListFromFileAsync();
        
        Subforum? subforum = subforums.SingleOrDefault(sf => sf.SubforumId == id);
        if (subforum is null) throw new InvalidOperationException($"Post with ID '{id}' not found");
        
        await UpdateFileAsync(subforums);
        
        return subforum;
    }

    public IQueryable<Subforum> GetMany()
    {
        List<Subforum> subforums = LoadListFromFileAsync().Result;
        return subforums.AsQueryable();
    }
}