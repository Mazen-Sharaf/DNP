using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class SubforumsController : ControllerBase
{
    private readonly ISubforumRepository _subforumRepository;
    private readonly IPostRepository _postRepository;

    public SubforumsController(ISubforumRepository subforumRepository, IPostRepository postRepository)
    {
        _subforumRepository = subforumRepository;
        _postRepository = postRepository;
    }

    private SubforumDTO ConvertEntityToDTO(Subforum subforum)
    {
        return new()
        {
            Name = subforum.Name,
            ModeratorId = subforum.ModeratorId,
            SubforumId = subforum.SubforumId
        };
    }

    private Subforum ConvertDTOToEntity(SubforumDTO subforumDTO)
    {
        return new()
        {
            Name = subforumDTO.Name,
            ModeratorId = subforumDTO.ModeratorId,
            SubforumId = subforumDTO.SubforumId
        };
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SubforumDTO>> GetSubforumByIdAsync([FromRoute] int id)
    {
        try
        {
            var subforum = await _subforumRepository.GetSingleAsync(id);
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
            var result = _subforumRepository.GetMany();

            if (name != null) result = result.Where(x => x.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
            if (moderatedBy != null) result = result.Where(x => x.ModeratorId == moderatedBy);

            return Ok(result.ToList());
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

            await _subforumRepository.DeleteAsync(id);

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
            await _subforumRepository.AddAsync(ConvertDTOToEntity(subforum));

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
            await _subforumRepository.UpdateAsync(ConvertDTOToEntity(subforum));

            return Ok("Subforum updated");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}