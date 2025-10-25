using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostRepository _posts;
    private readonly IReactionRepository _reactions;

    public PostsController(IPostRepository posts, IReactionRepository reactions)
    {
        _posts = posts;
        _reactions = reactions;
    }
    
    private Post DTOPostToEntity(PostDTO post)
    {
        return new Post()
        {
            Title = post.Title,
            AuthorId = post.AuthorId,
            CommentedOnPostId = post.CommentedOnPostId,
            Content = post.Content,
            SubforumId = post.SubforumId
        };
    }

    private PostDTO EntityPostToDTO(Post post)
    {
        return new PostDTO()
        {
            Title = post.Title,
            AuthorId = post.AuthorId,
            CommentedOnPostId = post.CommentedOnPostId,
            Content = post.Content,
            SubforumId = post.SubforumId
        };
    }
    
    [HttpPost]
    public async Task<ActionResult<PostDTO>> Create([FromBody] PostDTO post)
    {
        Post createdPost = await _posts.AddAsync(DTOPostToEntity(post));
        
        return Created($"/Posts/{createdPost.PostId}", createdPost);
    }

    [HttpPost("{id}")]
    public async Task<ActionResult<PostDTO>> Update([FromBody] PostDTO post)
    {
        await _posts.UpdateAsync(DTOPostToEntity(post));
        
        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PostDTO>> GetSingle([FromRoute] int id)
    {
        PostDTO post = EntityPostToDTO(await _posts.GetSingleAsync(id));

        return Ok(post);
    }

    [HttpGet("getmany")]
    public async Task<ActionResult<List<PostDTO>>> GetAll(  [FromQuery] string? title, 
                                                            [FromQuery] int? authorId, 
                                                            [FromQuery] int? commentedOnPostId,
                                                            [FromQuery] int? subforumId)
    {
        var posts = _posts.GetMany();
        
        if (title != null) posts = posts.Where(p => p.Title != null && p.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
        if (authorId != null) posts = posts.Where(p => p.AuthorId == authorId);
        if (commentedOnPostId != null) posts = posts.Where(p => p.CommentedOnPostId == commentedOnPostId);
        if (subforumId != null) posts = posts.Where(p => p.SubforumId == subforumId);
        
        
        return Ok(posts.AsQueryable().ToList());
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<PostDTO>> Delete([FromRoute] int id)
    {
        await _posts.DeleteAsync(id);
        
        return Ok();
    }

    [HttpPost("{id}/react")]
    public async Task<ActionResult<PostDTO>> Like([FromRoute] int id, [FromBody] UserDTO user, [FromQuery] string type)
    {
        try
        {
            Post post = await _posts.GetSingleAsync(id); // thrower hvis opslaget ikke findes

            await _reactions.AddAsync(new()
            {
                ByUserId = user.UserId,
                PostId = post.PostId,
                Type = type
            });

            return Ok();
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("{id}/react")]
    public async Task<ActionResult<PostDTO>> Dislike([FromRoute] int id, [FromBody] UserDTO user, [FromQuery] string type)
    {
        try
        {
            Post post = await _posts.GetSingleAsync(id); // thrower hvis opslaget ikke findes
            
            var reaction = _reactions.GetMany()
                .Where(r => r.PostId == post.PostId)
                .Where(r => r.Type == type)
                .FirstOrDefault(r => r.ByUserId == user.UserId);
            
            if (reaction == null) throw new InvalidOperationException("No matching reaction found");
            
            await _reactions.DeleteAsync(reaction);

            return Ok();
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("{id}/reactions")]
    public async Task<ActionResult<List<ReactionDTO>>> GetReactions([FromRoute] int id)
    {    
        var reactions = _reactions.GetMany().Where(r => r.PostId == id);

        return Ok(reactions);
    }
}