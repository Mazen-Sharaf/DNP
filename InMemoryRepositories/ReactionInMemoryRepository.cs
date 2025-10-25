using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class ReactionInMemoryRepository : IReactionRepository
{
    private List<Reaction> reactions = [];

    public Task<Reaction> AddAsync(Reaction reaction)
    {
        if (reactions.Any(r =>
                r.PostId == reaction.PostId && r.ByUserId == reaction.ByUserId && r.Type == reaction.Type))
        {
            throw new InvalidOperationException("Reaction already exists");
        }

        reactions.Add(reaction);
        return Task.FromResult(reaction);
    }

    public Task DeleteAsync(Reaction reaction)
    {
        Reaction? reactionToRemove = reactions.SingleOrDefault(r =>
            r.PostId == reaction.PostId && r.ByUserId == reaction.ByUserId && r.Type == reaction.Type);

        if (reactionToRemove == null) throw new InvalidOperationException("Reaction not found");

        reactions.Remove(reactionToRemove);

        return Task.CompletedTask;
    }

    public Task DeleteAllAsync(Post post)
    {
        reactions.RemoveAll(r => r.PostId == post.PostId);

        return Task.CompletedTask;
    }

    public Task DeleteAllAsync(User user)
    {
        reactions.RemoveAll(r => r.ByUserId == user.UserId);

        return Task.CompletedTask;
    }

    public Task<int> GetTotalOfTypeAsync(Post post, string type)
    {
        return Task.FromResult(reactions.Count(r => r.PostId == post.PostId && r.Type == type));
    }

    public IQueryable<Reaction> GetMany()
    {
        return reactions.AsQueryable();
    }
}