using Entities;
using RepositoryContracts;

namespace CLI.UI.Views.Users;

public class ListUser(ViewHandler viewHandler, IUserRepository userRepository) : IView
{
    private readonly ViewHandler _viewHandler = viewHandler;
    private readonly IUserRepository _userRepository = userRepository;

    public void Display()
    {
        Console.WriteLine("Users listed -->");
        Console.WriteLine("Type something to exit");
        Console.WriteLine(" ");

        var users = _userRepository.GetMany();
        foreach (var user in users)
        {
            Console.WriteLine($"- Username: {user.Username} - Id: {user.UserId}");
        }
    }

    public void HandleInput(string input)
    {
        _viewHandler.GoToMainView();
    }
}