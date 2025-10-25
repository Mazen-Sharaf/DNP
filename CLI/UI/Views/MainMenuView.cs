namespace CLI.UI.Views;

public class MainMenuView(ViewHandler handler) : IView
{
    public void Display()
    {
        Console.WriteLine("Welcome to the My Forum");
        Console.WriteLine("-------------------------------");
        Console.WriteLine("What page do you want to go to?");
        Console.WriteLine(" - Create User");
        Console.WriteLine(" - Manage User");
        Console.WriteLine(" - Delete User");
        Console.WriteLine(" - List Users");
        Console.WriteLine("");
        Console.WriteLine(" - Create subforum");
        Console.WriteLine(" - Manage subforum");
        Console.WriteLine(" - Delete subforum");
        Console.WriteLine(" - List subforums");
    }

    public async Task HandleInput(string input)
    {
        switch (input.ToLower())
        {
            case "create user":
                await handler.GoToView(Views.CreateUser);
                break;
            case "manage user":
                await handler.GoToView(Views.ManageUser);
                break;
            case "delete user":
                await handler.GoToView(Views.DeleteUser);
                break;
            case "list users":
                await handler.GoToView(Views.ListUsers);
                break;
            case "create subforum":
                await handler.GoToView(Views.CreateSubforum);
                break;
            case "manage subforum":
                await handler.GoToView(Views.ManageSubforum);
                break;
            case "delete subforum":
                await handler.GoToView(Views.DeleteSubforum);
                break;
            case "list subforums":
                await handler.GoToView(Views.ListSubforums);
                break;
            default:
                await handler.GoToMainMenu();
                break;
        }
    }
}