using System.Diagnostics.CodeAnalysis;
using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class SubforumsController : ControllerBase
{
    private readonly ISubforumRepository _subforums;
    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;

    public SubforumsController(ISubforumRepository subforums, IPostRepository postRepository,
        IUserRepository userRepository)
    {
        _subforums = subforums;
        _postRepository = postRepository;
        _userRepository = userRepository;
    }

    private SubforumDTO ConvertEntityToDTO(Subforum subforum)
    {
        return new()
        {
            Name = subforum.Name,
            ModeratorId = subforum.Moderator.UserId,
            SubforumId = subforum.SubforumId
        };
    }

    private async Task<Subforum> ConvertDTOToEntity(SubforumDTO subforumDTO)
    {
        return new()
        {
            Name = subforumDTO.Name,
            Moderator = await _userRepository.GetSingleAsyncById(subforumDTO.ModeratorId),
            SubforumId = subforumDTO.SubforumId
        };
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SubforumDTO>> GetSubforumByIdAsync([FromRoute] int id)
    {
        try
        {
            var subforum = await _subforums.GetSingleAsync(id);
            return Ok(ConvertEntityToDTO(subforum));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<SubforumDTO>>> GetSubforumsAsync([FromQuery] string? name,
        [FromQuery] int? moderatedBy)
    {
        try
        {
            var result = _subforums.GetMany();

            if (name != null) result = result.Where(x => x.Name.ToLower().Contains(name.ToLower()));
            if (moderatedBy != null) result = result.Where(x => x.Moderator.UserId == moderatedBy);

            var returnList = new List<SubforumDTO>();
            foreach (var subforum in result.ToList())
            {
                returnList.Add(new()
                {
                    Name = subforum.Name,
                    ModeratorId = subforum.Moderator.UserId,
                    SubforumId = subforum.SubforumId
                });
            }

            return Ok(returnList);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<SubforumDTO>> DeleteSubforumAsync([FromRoute] int id)
    {
        try
        {
            await _postRepository.DeleteAllFromSubforumAsync(id);

            await _subforums.DeleteAsync(id);

            return Ok("Subforum deleted");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult> CreateSubforumAsync([FromBody] SubforumDTO subforum)
    {
        try
        {
            await _subforums.AddAsync(await ConvertDTOToEntity(subforum));

            return Ok("Subforum created");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("{id}")]
    public async Task<ActionResult> UpdateSubforumAsync([FromBody] SubforumDTO subforum)
    {
        try
        {
            await _subforums.UpdateAsync(await ConvertDTOToEntity(subforum));

            return Ok("Subforum updated");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}