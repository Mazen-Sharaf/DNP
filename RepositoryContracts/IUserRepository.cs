namespace RepositoryContracts;

using Entities;

public interface IUserRepository
{
    Task<User> AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(int id);
    Task<User> GetSingleAsyncById(int id);
    Task<User> GetSingleAsyncByUsername(string username);
    IQueryable<User> GetMany();
    Task<Boolean> VerifyCredentialsAsync(string username, string password);
}