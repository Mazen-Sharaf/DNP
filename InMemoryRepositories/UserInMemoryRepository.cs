using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class UserInMemoryRepository : IUserRepository
{
    private List<User> users = [];

    public Task<User> AddAsync(User user)
    {
        user.UserId = users.Count != 0 ? users.Max(u => u.UserId) + 1 : 1;
        users.Add(user);
        return Task.FromResult(user);
    }

    public Task UpdateAsync(User user)
    {
        User? existingUser = users.SingleOrDefault(u => u.UserId == user.UserId);
        if (existingUser is null) throw new InvalidOperationException($"User with ID '{user.UserId}' not found");

        users.Remove(existingUser);
        users.Add(user);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        User? userToRemove = users.SingleOrDefault(u => u.UserId == id);
        if (userToRemove is null) throw new InvalidOperationException($"User with ID '{id}' not found");

        users.Remove(userToRemove);

        return Task.CompletedTask;
    }

    public Task<User> GetSingleAsyncById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetSingleAsync(int id)
    {
        User? user = users.SingleOrDefault(u => u.UserId == id);
        if (user is null) throw new InvalidOperationException($"User with ID '{id}' not found");

        return Task.FromResult(user);
    }

    public Task<User> GetSingleAsyncByUsername(string username)
    {
        throw new NotImplementedException();
    }

    public IQueryable<User> GetMany()
    {
        return users.AsQueryable();
    }

    public Task<bool> VerifyCredentialsAsync(string username, string password)
    {
        throw new NotImplementedException("Not implemented in InMemoryRepository");
    }
}