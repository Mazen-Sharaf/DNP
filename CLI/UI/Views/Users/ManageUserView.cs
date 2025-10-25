using Entities;
using RepositoryContracts;

namespace CLI.UI.Views.Users;

public class ManageUserView(ViewHandler viewHandler, IUserRepository userRepository) : IView
{
    public void Display()
    {
        Console.WriteLine("-- Manage User --");
        Console.WriteLine("To change a password, write username and new password");
        Console.WriteLine("Example: [Username] [New Password]");
        Console.WriteLine("Type 'exit' to exit.");
        Console.WriteLine("-----------------");
    }

    public async Task HandleInput(string input)
    {
        try
        {
            switch (input.ToLower())
            {
                case "exit":
                    await viewHandler.GoToMainMenu();
                    break;
                default:
                    var args = input.Split(' ');
                    if (args.Length != 2)
                        throw new ArgumentException("Invalid args");
                    var username = args[0];
                    var password = args[1];

                    await userRepository.UpdateAsync(new User { Username = username, Password = password });
                    Console.WriteLine($"User {username} changed password to {password}");
                    break;
            }
            
            await viewHandler.GoToMainMenu();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            await viewHandler.GoToView(Views.ManageUser);
        }
    }
}