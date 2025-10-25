using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IPostRepository _postRepository;

    public UsersController(IUserRepository userRepository, IPostRepository postRepository)
    {
        _userRepository = userRepository;
        _postRepository = postRepository;
    }

    private User DTOUserToEntity(UserDTO user)
    {
        return new()
        {
            Password = user.Password,
            UserId = user.UserId,
            Username = user.Username
        };
    }

    private UserDTO EntityUserToDTO(User user)
    {
        return new()
        {
            Password = user.Password,
            UserId = user.UserId,
            Username = user.Username
        };
    }

    [HttpPost]
    public async Task<ActionResult<UserDTO>> AddUser([FromBody] UserDTO userDTO)
    {
        User createdUser = await _userRepository.AddAsync(DTOUserToEntity(userDTO));

        return Created($"/Users/{createdUser.UserId}", EntityUserToDTO(createdUser));
    }

    [HttpGet]
    public async Task<ActionResult<List<UserDTO>>> GetMany([FromQuery] string? username)
    {
        var users = _userRepository.GetMany();

        if (username != null)
            users = users.Where(u => u.Username.Contains(username, StringComparison.OrdinalIgnoreCase));

        return Ok(users.ToList());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDTO>> GetById([FromRoute] int id)
    {
        return Ok(await _userRepository.GetSingleAsync(id));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<UserDTO>> Delete([FromRoute] int id)
    {
        await _postRepository.DeleteAllFromUserAsync(id);

        await _userRepository.DeleteAsync(id);

        return Ok("User deleted");
    }

    [HttpPost("update")]
    public async Task<ActionResult<UserDTO>> Update([FromBody] UserDTO userDTO)
    {
        await _userRepository.UpdateAsync(DTOUserToEntity(userDTO));

        return Ok("User updated");
    }
}