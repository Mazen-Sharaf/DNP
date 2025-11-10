using System.Security.Claims;
using System.Text.Json;
using ApiContracts;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace BlazorApp;

public class SimpleAuthProvider : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime;
    private ClaimsPrincipal? _currentClaimsPrincipal;

    private int _userId = -1;

    public SimpleAuthProvider(HttpClient httpClient, IJSRuntime jsRuntime)
    {
        _httpClient = httpClient;
        _jsRuntime = jsRuntime;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        string userAsJson = "";
        try
        {
            userAsJson = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "currentUser");
        }
        catch (InvalidOperationException e)
        {
            return new AuthenticationState(new());
        }

        if (string.IsNullOrEmpty(userAsJson))
        {
            return new AuthenticationState(new());
        }

        UserDTO userDto = JsonSerializer.Deserialize<UserDTO>(userAsJson)!;
        List<Claim> claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, userDto.Username),
            new Claim(ClaimTypes.NameIdentifier, userDto.UserId.ToString()),
        };

        ClaimsIdentity identity = new ClaimsIdentity(claims, "apiauth");
        _currentClaimsPrincipal = new ClaimsPrincipal(identity);

        return new AuthenticationState(_currentClaimsPrincipal ?? new ClaimsPrincipal());
    }

    public async Task Login(string username, string password)
    {
        var response = await _httpClient.PostAsJsonAsync(
            "Auth/login",
            new CreateUserDTO()
            {
                Username = username,
                Password = password
            });

        var content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        var userDto = JsonSerializer.Deserialize<UserDTO>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;

        string serialisedData = JsonSerializer.Serialize(userDto);
        await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser", serialisedData);

        List<Claim> claims =
        [
            new(ClaimTypes.Name, userDto.Username),
            new("id", userDto.UserId.ToString())
        ];

        var identity = new ClaimsIdentity(claims, "apiauth");
        _currentClaimsPrincipal = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(_currentClaimsPrincipal))
        );

        _userId = userDto.UserId;
    }

    public async void Logout()
    {
        await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser", "");
        _currentClaimsPrincipal = new();
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentClaimsPrincipal)));
        _userId = -1;
    }

    public int GetCurrentUserId()
    {
        return _userId == -1 ? throw new Exception("Ikke logget ind") : _userId;
    }
}