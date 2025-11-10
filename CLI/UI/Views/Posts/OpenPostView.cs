using Entities;
using RepositoryContracts;

namespace CLI.UI.Views.Posts;

public class OpenPostView(
    ViewHandler viewHandler,
    IPostRepository postRepository,
    IUserRepository userRepository,
    IReactionRepository reactionRepository)
    : IView
{
    public async void Display()
    {
        if (viewHandler.ViewState.CurrentPost is null)
        {
            await viewHandler.GoToMainMenu();
            return;
        }

        Console.WriteLine("Type 'exit' to exit.");
        Console.WriteLine("To comment, type comment, user id of author and the content of the comment");
        Console.WriteLine("Example: comment [id] [content]");
        Console.WriteLine("To react, type react, user id of author and like or dislike");
        Console.WriteLine("Example: react [id] like/dislike");
        Console.WriteLine("To remove, type remove");
        Console.WriteLine("Example: remove");
        Console.WriteLine("To remove a comment, type remove and id of comment");
        Console.WriteLine("Example: remove [comment id]");
        Console.WriteLine("");
        Console.WriteLine("");
        Console.WriteLine($"-- {viewHandler.ViewState.CurrentPost.Title} --");
        Console.WriteLine(
            $"Author: {userRepository.GetSingleAsyncById(viewHandler.ViewState.CurrentPost.AuthorId).Result.Username}");
        Console.WriteLine("-");
        Console.WriteLine(viewHandler.ViewState.CurrentPost.Content);
        Console.WriteLine("-----------------------------");

        // reactions
        var reactionsOfPost = reactionRepository.GetMany().ToList()
            .FindAll(r => r.PostId == viewHandler.ViewState.CurrentPost.PostId);
        var likes = reactionsOfPost.FindAll(r => r.Type == "like");
        var dislikes = reactionsOfPost.FindAll(r => r.Type == "dislike");
        Console.WriteLine($"Likes: {likes.Count} - Dislikes: {dislikes.Count}");
        Console.WriteLine("-----------------------------");


        // comments
        var commentsOnCurrentPost = postRepository.GetMany().ToList()
            .FindAll(c => c.CommentedOnPostId == viewHandler.ViewState.CurrentPost.PostId);
        foreach (var comment in commentsOnCurrentPost)
        {
            Console.WriteLine(
                $"    -- ID: {comment.PostId} | By: {userRepository.GetSingleAsyncById(comment.AuthorId).Result.Username}");
            Console.WriteLine($"        {comment.Content}");
            Console.WriteLine("");
        }
    }

    public async Task HandleInput(string input)
    {
        try
        {
            var loweredInput = input.ToLower();
            var loweredSplitInput = loweredInput.Split(" ");
            switch (loweredSplitInput[0])
            {
                case "exit":
                    await viewHandler.GoToView(Views.OpenSubforum);
                    break;
                case "comment":
                    var userId = int.Parse(loweredSplitInput[1]);
                    var content = input.Substring(loweredSplitInput[0].Length + loweredSplitInput[1].Length + 2);
                    await postRepository.AddAsync(new Post()
                    {
                        AuthorId = userId,
                        Content = content,
                        SubforumId = viewHandler.ViewState.CurrentPost!.SubforumId,
                        CommentedOnPostId = viewHandler.ViewState.CurrentPost.PostId,
                    });
                    await viewHandler.GoToView(Views.OpenPost);
                    break;
                case "react":
                    userId = int.Parse(loweredSplitInput[1]);

                    var reaction = loweredSplitInput[2];
                    if (reaction != "like" && reaction != "dislike")
                        throw new Exception("Invalid reaction");

                    await reactionRepository.AddAsync(new Reaction()
                    {
                        ByUserId = userId,
                        PostId = viewHandler.ViewState.CurrentPost!.PostId,
                        Type = reaction
                    });
                    await viewHandler.GoToView(Views.OpenPost);
                    break;
                case "remove":
                    if (loweredSplitInput.Length == 1)
                    {
                        await postRepository.DeleteAsync(viewHandler.ViewState.CurrentPost!.PostId);
                        await viewHandler.GoToView(Views.OpenSubforum);
                    }
                    else
                    {
                        var commentId = int.Parse(loweredSplitInput[1]);
                        await postRepository.DeleteAsync(commentId);

                        await viewHandler.GoToView(Views.OpenPost);
                    }

                    break;
            }
        }
        catch (Exception e)
        {
            viewHandler.GoToView(Views.OpenPost);
            Console.WriteLine(e);
        }
    }
}