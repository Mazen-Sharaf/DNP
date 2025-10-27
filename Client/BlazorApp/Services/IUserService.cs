using ApiContracts;

namespace BlazorApp.Services;

public interface IUserService
{
    public Task<UserDTO> AddUserAsync(CreateUserDTO user);
    public Task UpdateUserAsync(int id, CreateUserDTO user);
}