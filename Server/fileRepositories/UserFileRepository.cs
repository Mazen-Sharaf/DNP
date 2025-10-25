using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class UserFileRepository : IUserRepository
{
    private readonly string _filePath = "users.json";

    public UserFileRepository()
    {
        if (!File.Exists(_filePath))
        {
            File.WriteAllText(_filePath, "[]");
        }
    }

    private async Task<List<User>> LoadListFromFileAsync()
    {
        string json = await File.ReadAllTextAsync(_filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(json)!;
        return users;
    }

    private async Task UpdateFileAsync(List<User> users)
    {
        string usersAsJson = JsonSerializer.Serialize(users);
        await File.WriteAllTextAsync(_filePath, usersAsJson);
    }
    public async Task<User> AddAsync(User user)
    {
        List<User> users = LoadListFromFileAsync().Result;
        
        user.UserId = users.Count != 0 ? users.Max(u => u.UserId) + 1 : 1;
        users.Add(user);
        
        await UpdateFileAsync(users);
        
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        List<User> users = LoadListFromFileAsync().Result;
        
        User? existingUser = users.SingleOrDefault(u => u.UserId == user.UserId);
        if (existingUser is null) throw new InvalidOperationException($"User with ID '{user.UserId}' not found");

        users.Remove(existingUser);
        users.Add(user);
        
        await UpdateFileAsync(users);
    }

    public async Task DeleteAsync(int id)
    {
        List<User> users = LoadListFromFileAsync().Result;
        
        User? userToRemove = users.SingleOrDefault(u => u.UserId == id);
        if (userToRemove is null) throw new InvalidOperationException ($"User with ID '{id}' not found");

        users.Remove(userToRemove);
        
        await UpdateFileAsync(users);
    }

    public async Task<User> GetSingleAsync(int id)
    {
        List<User> users = LoadListFromFileAsync().Result;
        
        User? user = users.SingleOrDefault(u => u.UserId == id);
        if (user is null) throw new InvalidOperationException($"User with ID '{id}' not found");

        await UpdateFileAsync(users);
        
        return user;
    }

    public IQueryable<User> GetMany()
    {
        List<User> users = LoadListFromFileAsync().Result;
        return users.AsQueryable();
    }

    public async Task<bool> VerifyCredentialsAsync(string username, string password)
    {
        List<User> users = await LoadListFromFileAsync();
        return users.Any(u => u.Username == username && u.Password == password);
    }
}