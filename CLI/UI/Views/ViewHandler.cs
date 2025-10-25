using CLI.UI.Views.Subforums;
using CLI.UI.Views.Posts;
using CLI.UI.Views.Users;
using Entities;
using RepositoryContracts;

namespace CLI.UI.Views;

public class ViewHandler
{
    private readonly IPostRepository _posts;
    private readonly IUserRepository _users;
    private readonly ISubforumRepository _subforums;
    private readonly IReactionRepository _reactions;
    
    private readonly MainMenuView _mainMenuView;
    
    private readonly CreateUserView _createUserView;
    private readonly ManageUserView _manageUserView;
    private readonly DeleteUserView _deleteUserView;
    private readonly ListUserView _listUserView;
    
    private readonly CreateSubforumView _createSubforumView;
    private readonly ManageSubforumView _manageSubforumView;
    private readonly ListSubforumView _listSubforumView;
    private readonly DeleteSubforumView _deleteSubforumView;
    private readonly OpenSubforumView _openSubforumView;
    
    private readonly OpenPostView _openPostView;

    public ViewState ViewState { get; } = new ViewState();

    public ViewHandler (IPostRepository posts, 
                        IUserRepository users, 
                        ISubforumRepository subforums, 
                        IReactionRepository reactions)
    {
        _posts = posts;
        _users = users;
        _subforums = subforums;
        _reactions = reactions;

        _mainMenuView = new MainMenuView(this);
        
        _createUserView = new CreateUserView(this, _users);
        _manageUserView = new ManageUserView(this, _users);
        _deleteUserView = new DeleteUserView(this, _users, _posts);
        _listUserView = new ListUserView(this, _users);

        _createSubforumView = new CreateSubforumView(this, _subforums, _users);
        _manageSubforumView = new ManageSubforumView(this, _subforums);
        _deleteSubforumView = new DeleteSubforumView(this, _subforums, _posts);
        _listSubforumView = new ListSubforumView(this, _subforums);
        _openSubforumView = new OpenSubforumView(this, _posts, _users);

        _openPostView = new OpenPostView(this, _posts, _users, _reactions);
    }

    public async Task GoToView(Views viewName)
    {
        IView view;
        switch (viewName)
        {
            case Views.CreateUser:
                view = _createUserView;
                break;
            case Views.ManageUser:
                view = _manageUserView;
                break;
            case Views.DeleteUser:
                view = _deleteUserView;
                break;
            case Views.ListUsers:
                view = _listUserView;
                break;
            case Views.CreateSubforum:
                view = _createSubforumView;
                break;
            case Views.ManageSubforum:
                view = _manageSubforumView;
                break;
            case Views.DeleteSubforum:
                view = _deleteSubforumView;
                break;
            case Views.ListSubforums:
                view = _listSubforumView;
                break;
            case Views.OpenSubforum:
                view = _openSubforumView;
                break;
            case Views.OpenPost:
                view = _openPostView;
                break;
            default:
                view = _mainMenuView;
                break;
        }
        await ShowView(view);
    }
    
    private async Task ShowView(IView view)
    {
        Console.Clear();
        view.Display();
        while (true)
        {
            Console.Write("> ");
            string? input = Console.ReadLine();
            if (input is null)
                continue;
            
            await view.HandleInput(input);
            break;
        }
    }

    public async Task GoToMainMenu()
    {
        await ShowView(_mainMenuView);
    }
}

public enum Views
{
    MainMenu,
    
    CreateUser,
    ManageUser,
    DeleteUser,
    ListUsers,
    
    CreateSubforum,
    ManageSubforum,
    DeleteSubforum,
    ListSubforums,
    OpenSubforum,
    
    OpenPost
}

public class ViewState
{
    public Subforum? CurrentSubforum { get; set; }
    public Post? CurrentPost { get; set; }
}