using ApiContracts;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public AuthController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDTO>> Login([FromBody] CreateUserDTO createUserDto)
    {
        try
        {
            if (await _userRepository.VerifyCredentialsAsync(createUserDto.Username, createUserDto.Password))
            {
                var user = await _userRepository.GetSingleAsyncByUsername(createUserDto.Username);
                return Ok(new UserDTO()
                {
                    Username = user.Username,
                    UserId = user.UserId
                });
            }
        }
        catch (Exception e)
        {
            // Gør ingenting
        }

        return Unauthorized();
    }
}