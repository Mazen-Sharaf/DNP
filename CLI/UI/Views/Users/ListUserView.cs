using Entities;
using RepositoryContracts;

namespace CLI.UI.Views.Users;

public class ListUserView(ViewHandler viewHandler, IUserRepository userRepository) : IView
{
    public void Display()
    {
        Console.WriteLine("-- Users listed --");
        Console.WriteLine("Type 'exit' to exit.");
        Console.WriteLine("------------------");

        var users = userRepository.GetMany();
        foreach (var user in users)
        {
            Console.WriteLine($"- Username: {user.Username} - Id: {user.UserId}");
        }

    }

    public async Task HandleInput(string input)
    {
        // Den er ligeglad om hvad du skriver
        await viewHandler.GoToMainMenu();
    }
}