namespace RepositoryContracts;

using Entities;

public interface IPostRepository
{
    Task<Post> AddAsync(Post post);
    Task UpdateAsync(Post post);
    Task DeleteAsync(int id);
    Task DeleteAllFromSubforumAsync(int subforumId);
    Task DeleteAllFromUserAsync(int userId);
    Task<Post> GetSingleAsync(int id);
    IQueryable<Post> GetMany();
}