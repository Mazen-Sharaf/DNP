namespace RepositoryContracts;

using Entities;

public interface ISubforumRepository
{
    Task<Subforum> AddAsync(Subforum subforum);
    Task UpdateAsync(Subforum subforum);
    Task DeleteAsync(int id);
    Task<Subforum> GetSingleAsync(int id);
    IQueryable<Subforum> GetMany();
}