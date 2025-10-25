using Entities;
using RepositoryContracts;

namespace CLI.UI.Views.Subforums;

public class OpenSubforumView(
    ViewHandler viewHandler,
    IPostRepository postRepository,
    IUserRepository userRepository)
    : IView
{
    public void Display()
    {
        // Null tjek
        if (viewHandler.ViewState.CurrentSubforum is null)
        {
            viewHandler.GoToMainMenu();
            Console.WriteLine("No subforum selected");
            return;
        }

        Console.WriteLine($"-- {viewHandler.ViewState.CurrentSubforum.Name} --");
        Console.WriteLine(
            $"Moderated by: {userRepository.GetSingleAsync(viewHandler.ViewState.CurrentSubforum.ModeratorId).Result.Username}");
        Console.WriteLine("");
        Console.WriteLine(
            "To create a post, write create, the userid of the author, and the title of the post, press enter and then type the content of the post");
        Console.WriteLine("Example: create [Author id] [Title]");
        Console.WriteLine("Example: [content]");
        Console.WriteLine("");
        Console.WriteLine("To open a post, enter open and the id of the post");
        Console.WriteLine("Example: open [Post id]");
        Console.WriteLine("Type 'exit' to exit.");
        Console.WriteLine("_________________________________________________");

        // Henter alle posts fra dette forum
        var posts = postRepository.GetMany().ToList()
            .FindAll(p => p.SubforumId == viewHandler.ViewState.CurrentSubforum.SubforumId);

        var comments = posts.FindAll(p => p.CommentedOnPostId != null);
        posts.RemoveAll(p => comments.Contains(p));

        if (posts.Count == 0)
            Console.WriteLine("No one has posted yet, be the first! Type create [author id] [title]");

        foreach (var post in posts)
        {
            Console.WriteLine($"-- {post.Title} --");
            Console.WriteLine(
                $"By: {userRepository.GetSingleAsync(post.AuthorId).Result.Username} - Post ID: {post.PostId}");
            Console.WriteLine("-");
            Console.WriteLine($"{post.Content}");
            Console.WriteLine("-");
            Console.WriteLine("");
        }
    }

    public async Task HandleInput(string input)
    {
        var inputLowerCase = input.ToLower();
        try
        {
            if (inputLowerCase == "exit")
            {
                await viewHandler.GoToView(Views.ListSubforums);
                return;
            }

            var splitInput = inputLowerCase.Split(" ");
            switch (splitInput[0])
            {
                case "create":
                    var authorId = int.Parse(splitInput[1]);
                    var substringCutIndex = splitInput[0].Length + splitInput[1].Length + 2;
                    var title = input.Substring(substringCutIndex);
                    string? content;

                    bool proceed = true;
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine($"-- Write post: {title} --");
                        Console.WriteLine("Type 'cancel' to exit.");
                        Console.WriteLine("__________________________");

                        content = Console.ReadLine();
                        if (content is null) continue;

                        if (content.ToLower() == "cancel")
                        {
                            proceed = false;
                            await viewHandler.GoToView(Views.ListSubforums);
                        }

                        break;
                    }

                    // Hvis man har cancelled, skal den break
                    if (!proceed) break;

                    await postRepository.AddAsync(new Post()
                    {
                        AuthorId = authorId,
                        Content = content,
                        SubforumId = viewHandler.ViewState.CurrentSubforum!.SubforumId,
                        Title = title
                    });
                    break;
                case "open":
                    var postId = int.Parse(splitInput[1]);
                    viewHandler.ViewState.CurrentPost = await postRepository.GetSingleAsync(postId);
                    await viewHandler.GoToView(Views.OpenPost);
                    break;
            }

            await viewHandler.GoToView(Views.OpenSubforum);
        }
        catch (Exception e)
        {
            await viewHandler.GoToView(Views.OpenSubforum);
            Console.WriteLine(e.Message);
        }
    }
}