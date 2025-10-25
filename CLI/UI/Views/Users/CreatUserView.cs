using Entities;
using RepositoryContracts;

namespace CLI.UI.Views.Users;

public class CreateUserView(ViewHandler viewHandler, IUserRepository userRepository) : IView
{
    public void Display()
    {
        Console.WriteLine("-- Create User --");
        Console.WriteLine("To create a new user, enter a username and password, seperate them with spaces");
        Console.WriteLine("Example: [username] [password]");
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

                    if (userRepository.GetMany().Any(u => u.Username == username))
                        break;

                    await userRepository.AddAsync(new User()
                    {
                        Username = username,
                        Password = password
                    });
                    Console.WriteLine($"User {username} added");
                    break;
            }
            
            await viewHandler.GoToMainMenu();
        }
        catch (Exception e)
        {
            await viewHandler.GoToView(Views.CreateUser);
            Console.WriteLine(e.Message);
        }
    }
}