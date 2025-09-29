using CLI.UI.Views.Posts;
using CLI.UI.Views.Subforum;
using CLI.UI.Views.Users;
using Entities;
using RepositoryContracts;

namespace CLI.UI.Views;

public class ViewHandler
{
    private IPostRepository _posts;
    private IUserRepository _users;
    private ISubforumRepository _subforums;

    private MainView _mainView;

    private CreateUser _createUserView;
    private DeleteUser _deleteUserView;
    private ListUser _listUserView;

    private CreateSubforum _createSubforumView;

    private PostView _postView;

    public ViewState ViewState { get; } = new ViewState();

    public ViewHandler(IPostRepository posts,
        IUserRepository users, ISubforumRepository subforumRepository)
    {
        _posts = posts;
        _users = users;
        _subforums = subforumRepository;

        _mainView = new MainView(this);

        _createUserView = new CreateUser(this, _users);
        _deleteUserView = new DeleteUser(this, _users, _posts);
        _listUserView = new ListUser(this, _users);
        
        _createSubforumView = new CreateSubforum(this, _subforums, _users);

        _postView = new PostView(this, _subforums, _posts, _users);
    }

    public void GoToView(Views viewName)
    {
        IView view;
        switch (viewName)
        {
            case Views.CreateUser:
                view = _createUserView;
                break;
            case Views.DeleteUser:
                view = _deleteUserView;
                break;
            case Views.ListUsers:
                view = _listUserView;
                break;
            case Views.PostView:
                view = _postView;
                break;
            case Views.CreateSubforum:
                view = _createSubforumView;
                break;
            default:
                view = _mainView;
                break;
        }

        ShowView(view);
    }

    private void ShowView(IView view)
    {
        Console.Clear();
        view.Display();
        while (true)
        {
            Console.Write(" ");
            string? input = Console.ReadLine();
            if (input is null)
                continue;

            view.HandleInput(input);
            break;
        }
    }

    public void GoToMainView()
    {
        ShowView(new MainView(this));
    }
}

public enum Views
{
    MainView,
    CreateUser,
    DeleteUser,
    ListUsers,
    CreateSubforum,
    PostView,
}

public class ViewState
{
    public Entities.Subforum? CurrentSubforum { get; set; }
    public Post? CurrentPost { get; set; }
}