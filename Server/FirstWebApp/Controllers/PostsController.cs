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
            SubforumId = post.SubforumId,
            PostId = post.PostId
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
            SubforumId = post.SubforumId,
            PostId = post.PostId
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
    public async Task<ActionResult<List<PostDTO>>> GetAll([FromQuery] string? title,
        [FromQuery] int? authorId,
        [FromQuery] int? commentedOnPostId,
        [FromQuery] int? subforumId)
    {
        var posts = _posts.GetMany();

        if (title != null)
            posts = posts.Where(p => p.Title != null && p.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
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
    public async Task<ActionResult<PostDTO>> Like([FromRoute] int id, [FromBody] ReactionDTO reaction)
    {
        try
        {
            Post post = await _posts.GetSingleAsync(id); // thrower hvis opslaget ikke findes

            await _reactions.AddAsync(new()
            {
                ByUserId = reaction.ByUserId,
                PostId = reaction.PostId,
                Type = reaction.Type,
            });

            return Ok();
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("{id}/react")]
    public async Task<ActionResult<PostDTO>> Dislike([FromRoute] int id, [FromBody] ReactionDTO reaction)
    {
        try
        {
            Post post = await _posts.GetSingleAsync(id); // thrower hvis opslaget ikke findes

            var targetReaction = _reactions.GetMany()
                .Where(r => r.PostId == reaction.PostId)
                .Where(r => r.Type == reaction.Type)
                .FirstOrDefault(r => r.ByUserId == reaction.ByUserId);

            if (targetReaction == null) throw new InvalidOperationException("No matching reaction found");

            await _reactions.DeleteAsync(targetReaction);

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