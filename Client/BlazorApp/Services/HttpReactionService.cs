using ApiContracts;

namespace BlazorApp.Services;

public class HttpReactionService : IReactionService
{
    private readonly HttpClient _httpClient;

    public HttpReactionService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<List<ReactionDTO>> GetAllReactionsOnPost(int postId)
    {
        List<ReactionDTO>? reactions = await _httpClient.GetFromJsonAsync<List<ReactionDTO>>($"Posts/{postId}/reactions");
        
        return reactions ?? [];
    }

    public async Task ReactOnPost(ReactionDTO reaction)
    {
        HttpResponseMessage res = await _httpClient.PostAsJsonAsync($"Posts/{reaction.PostId}/react", reaction);
        string responseString = await res.Content.ReadAsStringAsync();

        if (!res.IsSuccessStatusCode) throw new Exception(responseString);
    }

    public async Task RemoveReactionFromPost(ReactionDTO reaction)
    {
        HttpRequestMessage req = new HttpRequestMessage()
        {
            Content = JsonContent.Create(reaction),
            Method = HttpMethod.Delete,
            RequestUri = new Uri($"Posts/{reaction.PostId}/react", UriKind.Relative)
        };

        HttpResponseMessage res = await _httpClient.SendAsync(req);
        string responseString = await res.Content.ReadAsStringAsync();

        if (!res.IsSuccessStatusCode) throw new Exception(responseString);
    }
}