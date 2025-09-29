using System.Diagnostics;
using Entities;
using RepositoryContracts;

namespace CLI.UI.Views.Posts;

public class PostView(
    ViewHandler viewHandler,
    ISubforumRepository subforumRepository,
    IPostRepository postRepository,
    IUserRepository userRepository) : IView
{
    private readonly ViewHandler _viewHandler = viewHandler;
    private readonly ISubforumRepository _subforumRepository = subforumRepository;
    private readonly IPostRepository _postRepository = postRepository;
    private readonly IUserRepository _userRepository = userRepository;

    public void Display()
    {
        if (_viewHandler.ViewState.CurrentPost is null)
        {
            _viewHandler.GoToMainView();
            return;
        }

        Console.WriteLine("Type 0 to exit");
        Console.WriteLine("to comment, please type 1, and then the userId of the author and your comment");
        Console.WriteLine("if You wish to remove then type 2 ");
        Console.WriteLine("if you wish to remove a comment then type 3 and id of the comment");
        Console.WriteLine("");
        Console.WriteLine($"--{_viewHandler.ViewState.CurrentPost.Title}--");
        Console.WriteLine(
            $"Author: {_userRepository.GetSingleAsync(_viewHandler.ViewState.CurrentPost.AuthorId).Result.Username}");
        Console.WriteLine("");
        Console.WriteLine(_viewHandler.ViewState.CurrentPost.Content);
        Console.WriteLine("");

        var commentOnCurrentPost = _postRepository.GetMany().ToList()
            .FindAll(c => c.CommentedOnPostId == _viewHandler.ViewState.CurrentPost.PostId);
        foreach (var comment in commentOnCurrentPost)
        {
            Console.WriteLine(
                $" --ID: {comment.PostId} | By: {_userRepository.GetSingleAsync(comment.AuthorId).Result.Username}");
            Console.WriteLine($" {comment.Content}");
            Console.WriteLine("");
        }
    }

    public void HandleInput(string input)
    {
        try
        {
            switch (input)
            {
              case "0":
                  _viewHandler.GoToMainView();
                  break;
              case "1":
                // for at kommentere
                  break;
              case "2":
                  // for at slette en post
                  break;
              case "3":
                  // for at slette en kommentar
                break;
            } 
        }
        catch (Exception e)
        {
            _viewHandler.GoToView(Views.MainView);
        }
    }
}