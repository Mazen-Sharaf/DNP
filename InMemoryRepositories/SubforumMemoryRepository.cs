using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class SubforumInMemoryContract : ISubforumRepository
{
    private List<Subforum> subforums = [];

    public Task<Subforum> AddAsync(Subforum subforum)
    {
        subforum.SubforumId = subforums.Count != 0 ? subforums.Max(sf => sf.SubforumId) + 1 : 1;
        subforums.Add(subforum);
        return Task.FromResult(subforum);
    }

    public Task UpdateAsync(Subforum subforum)
    {
        Subforum? existingSubforum = subforums.SingleOrDefault(sf => sf.SubforumId == subforum.SubforumId);
        if (existingSubforum is null)
            throw new InvalidOperationException($"post with ID '{subforum.SubforumId}' not found");

        subforums.Remove(existingSubforum);
        subforums.Add(subforum);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        Subforum? subforumToRemove = subforums.SingleOrDefault(sf => sf.SubforumId == id);
        if (subforumToRemove is null) throw new InvalidOperationException($"Post with ID '{id}' not found");

        subforums.Remove(subforumToRemove);

        return Task.CompletedTask;
    }

    public Task<Subforum> GetSingleAsync(int id)
    {
        Subforum? subforum = subforums.SingleOrDefault(sf => sf.SubforumId == id);
        if (subforum is null) throw new InvalidOperationException($"Post with ID '{id}' not found");

        return Task.FromResult(subforum);
    }

    public IQueryable<Subforum> GetMany()
    {
        return subforums.AsQueryable();
    }
}