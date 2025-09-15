using Entities;

namespace InMemoryRepositories;

public class SubforumMemoryRepository
{
    private List<Subforum> _subforums = [];
    public Task<Subforum> AddAsync(Subforum subforum)
    {
        subforum.SubforumId = _subforums.Any()
            ? _subforums.Max(s => s.SubforumId) + 1
            : 1;
        _subforums.Add(subforum);
        return Task.FromResult(subforum);
    }
    public Task UpdateAsync(Subforum subforum)
    {
        Subforum? existingSubforum = _subforums.SingleOrDefault(s => s.SubforumId == subforum.SubforumId);
        if (existingSubforum is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{subforum.SubforumId}' not found");
        }

        _subforums.Remove(existingSubforum);
        _subforums.Add(subforum);

        return Task.CompletedTask;
    }
    public Task DeleteAsync(int id)
    {
        Subforum? subforumToRemove = _subforums.SingleOrDefault(s => s.SubforumId == id);
        if (subforumToRemove is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{id}' not found");
        }

        _subforums.Remove(subforumToRemove);
        return Task.CompletedTask;
    }

    public Task<Subforum> GetSingleAsync(int id)
    {
        Subforum? subforumToReturn = _subforums.SingleOrDefault(s => s.SubforumId == id);
        if (subforumToReturn is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{id}' not found");
        }

        return Task.FromResult(subforumToReturn);
    } 
    public IQueryable<Subforum> GetMany()
    {
        return _subforums.AsQueryable();
    }
}