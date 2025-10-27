using System.Text.Json;
using ApiContracts;

namespace BlazorApp.Services;

public class HttpUserService :IUserService
{
    private readonly HttpClient _httpClient;

    public HttpUserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<UserDTO> AddUserAsync(CreateUserDTO user)
    {
        HttpResponseMessage httpResponse = await _httpClient.PostAsJsonAsync("Users", user);
        string response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode) throw new Exception(response);
        
        return JsonSerializer.Deserialize<UserDTO>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public Task UpdateUserAsync(int id, CreateUserDTO user)
    {
        throw new NotImplementedException();
    }

    public async Task<UserDTO> GetUserAsync(int id)
    {
        UserDTO? res = await _httpClient.GetFromJsonAsync<UserDTO>($"Users/{id}");
        
        if (res == null) throw new KeyNotFoundException();

        return res;
    }
}