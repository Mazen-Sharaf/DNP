using ApiContracts;

namespace BlazorApp.Services;

public interface ISubforumService
{
    Task<List<SubforumDTO>> GetAllSubforumsAsync(SubforumSearchFilter filter);
    Task<SubforumDTO> GetSubforumAsync(int id);
    Task AddSubforumAsync(CreateSubforumDTO subforum);
}

public class SubforumSearchFilter
{
    public string? Name { get; set; }
    public Int32? ModeratedById { get; set; }

    public static SubforumSearchFilter NameSearch(string name)
    {
        return new SubforumSearchFilter { Name = name };
    }

    public static SubforumSearchFilter ModeratorSearch(Int32? id)
    {
        return new SubforumSearchFilter { ModeratedById = id };
    }
}