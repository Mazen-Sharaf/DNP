using Entities;
using RepositoryContracts;

namespace CLI.UI.Views.Users;

public class CreateUser(ViewHandler viewHandler, IUserRepository userRepository) : IView
{
    private ViewHandler viewHandler = viewHandler;
    private IUserRepository userRepository = userRepository;

    public void Display()
    {
        Console.WriteLine("Create User -->");
        Console.WriteLine("Enter a username, then space and Enter a password ");
        Console.WriteLine("type '0' to exit ");
    }

    public void HandleInput(string input)
    {
        try
        {
            switch (input.ToLower())
            {
                case "0":
                    viewHandler.GoToMainView();
                    break;
                default:
                    var args = input.Split(' ');
                    if (args.Length != 2)
                        throw new ArgumentException("Invalid!");
                    var username = args[0];
                    var password = args[1];

                    if (userRepository.GetMany().Any(u => u.Username == username))
                        break;

                    userRepository.AddAsync(new User()
                    {
                        Username = username,
                        Password = password
                    });
                    Console.WriteLine($"User {username} added");
                    break;
            }
        }
        catch (Exception e)
        {
            viewHandler.GoToView(Views.CreateUser);
        }
    }
}