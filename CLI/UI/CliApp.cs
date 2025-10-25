using System.Runtime.InteropServices;
using CLI.UI.Views;
using CLI.UI.Views.Users;
using Entities;
using RepositoryContracts;

namespace CLI.UI;

public class CLIApp
{
    private IPostRepository _posts;
    private IUserRepository _users;
    private ISubforumRepository _subforums;
    private IReactionRepository _reactions;

    private ViewHandler handler;

    public CLIApp(IPostRepository posts, IUserRepository users, ISubforumRepository subforums, IReactionRepository reactions)
    {
        _posts = posts;
        _users = users;
        _subforums = subforums;
        _reactions = reactions;
        
        handler = new ViewHandler(posts, users, subforums, reactions);
    }

    public async Task StartAsync()
    {
        await handler.GoToView(Views.Views.MainMenu);
    }
}