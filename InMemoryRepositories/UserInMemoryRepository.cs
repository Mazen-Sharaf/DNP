using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class UserInMemoryRepository
{
    
    private List<User> _users = [];
    public Task<User> AddAsync(User user)
    {
        user.UserId = _users.Any()
            ? _users.Max(u => u.UserId) + 1
            : 1;
        _users.Add(user);
        return Task.FromResult(user);
    }
    public Task UpdateAsync(User user)
    {
        User? existingUser = _users.SingleOrDefault(u => u.UserId == user.UserId);
        if (existingUser is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{user.UserId}' not found");
        }

        _users.Remove(existingUser);
        _users.Add(user);

        return Task.CompletedTask;
    }
    public Task DeleteAsync(int id)
    {
        User? userToRemove = _users.SingleOrDefault(u => u.UserId == id);
        if (userToRemove is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{id}' not found");
        }

        _users.Remove(userToRemove);
        return Task.CompletedTask;
    }

    public Task<User> GetSingleAsync(int id)
    {
        User? userToReturn = _users.SingleOrDefault(u => u.UserId == id);
        if (userToReturn is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{id}' not found");
        }

        return Task.FromResult(userToReturn);
    } 
    public IQueryable<User> GetMany()
    {
        return _users.AsQueryable();
    }
    
}