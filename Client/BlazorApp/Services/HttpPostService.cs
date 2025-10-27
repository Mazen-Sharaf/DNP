using System.Text.Json;
using ApiContracts;

namespace BlazorApp.Services;

public class HttpPostService : IPostService
{
    private readonly HttpClient _httpClient;

    public HttpPostService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<PostDTO>> GetAllPostsAsync(PostSearchFilter filter)
    {
        string requestUri = "/Posts/getmany?";

        if (filter.Title != null) requestUri += $"title={filter.Title}&";
        if (filter.AuthorId != null) requestUri += $"authorId={filter.AuthorId}&";
        if (filter.SubforumId != null) requestUri += $"subforumId={filter.SubforumId}&";
        if (filter.CommentedOnPostId != null) requestUri += $"commentedOnPostId={filter.CommentedOnPostId}&";

        List<PostDTO>? posts = await _httpClient.GetFromJsonAsync<List<PostDTO>>(requestUri);

        if (posts == null) posts = new List<PostDTO>();

        return posts;
    }

    public async Task<PostDTO> GetPostByIdAsync(int id)
    {
        PostDTO? res = await _httpClient.GetFromJsonAsync<PostDTO>($"Posts/{id}");

        if (res == null) throw new KeyNotFoundException();

        return res;
    }

    public async Task<PostDTO> AddPostAsync(CreatePostDTO postDto)
    {
        HttpResponseMessage httpResponse = await _httpClient.PostAsJsonAsync("Posts", postDto);
        string response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode) throw new Exception(response);

        return JsonSerializer.Deserialize<PostDTO>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }
}