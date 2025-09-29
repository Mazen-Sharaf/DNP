using Entities;

namespace CLI.UI.Views;

public class MainView : IView
{
    private ViewHandler _viewHandler;

    public MainView(ViewHandler viewHandler)
    {
        this._viewHandler = viewHandler;
    }

    public void Display()
    {
        Console.WriteLine("Welcome to my social application ");
        Console.WriteLine("");
        Console.WriteLine("Choose a number ");
        Console.WriteLine(" 1 -Create User");
        Console.WriteLine(" 2 -Delete User");
        Console.WriteLine(" 3 -List Users");
        Console.WriteLine("");
        Console.WriteLine(" 4 -List subforum");
        /*
        Console.WriteLine("  -Create subforum");
        Console.WriteLine("  -Manage subforum");
        Console.WriteLine("  -Delete subforum");
        */
    }

    public void HandleInput(string input)
    {
        switch (input)
        {
            case "1":
                _viewHandler.GoToView(Views.CreateUser);
                break;
            case "2":
                _viewHandler.GoToView(Views.DeleteUser);
                break;
            case "3":
                _viewHandler.GoToView(Views.ListUsers);
                break;
            /*case "4":
                _viewHandler.GoToView(Views.CreateSubforum);
                break;
            case "5":
                _viewHandler.GoToView(Views.ManageSubforum);
                break;
            case "6":
                _viewHandler.GoToView(Views.DeleteSubforum);
                break;
            case "7":
                _viewHandler.GoToView(Views.ListSubforums);
                break;
                */
            default:
                _viewHandler.GoToMainView();
                break;
        }
    }
}