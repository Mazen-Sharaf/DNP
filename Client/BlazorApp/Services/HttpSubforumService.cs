using ApiContracts;

namespace BlazorApp.Services;

public class HttpSubforumService : ISubforumService
{
    private readonly HttpClient _httpClient;

    public HttpSubforumService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<SubforumDTO>> GetAllSubforumsAsync(SubforumSearchFilter filter)
    {
        string requestUri = "/Subforums?";

        if (filter.Name != null) requestUri += $"name={filter.Name}&";
        if (filter.ModeratedById != null) requestUri += $"moderatedBy={filter.ModeratedById}&";

        List<SubforumDTO>? posts = await _httpClient.GetFromJsonAsync<List<SubforumDTO>>(requestUri);

        if (posts == null) posts = new List<SubforumDTO>();

        return posts;
    }

    public async Task<SubforumDTO> GetSubforumAsync(int id)
    {
        SubforumDTO? res = await _httpClient.GetFromJsonAsync<SubforumDTO>($"Subforums/{id}");

        if (res == null) throw new KeyNotFoundException();

        return res;
    }

    public Task AddSubforumAsync(CreateSubforumDTO subforum)
    {
        throw new NotImplementedException();
    }
}