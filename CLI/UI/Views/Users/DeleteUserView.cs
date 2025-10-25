using RepositoryContracts;

namespace CLI.UI.Views.Users;

public class DeleteUserView(ViewHandler viewHandler, IUserRepository userRepository, IPostRepository postRepository)
    : IView
{
    public void Display()
    {
        Console.WriteLine("-- Delete User --");
        Console.WriteLine("To delete user, write id of user");
        Console.WriteLine("Example: [id]");
        Console.WriteLine("Type 'exit' to exit.");
        Console.WriteLine("-----------------");
    }

    public async Task HandleInput(string input)
    {
        try
        {
            switch (input)
            {
                case "exit":
                    break;
                default:
                    int userId = int.Parse(input);
                    await userRepository.DeleteAsync(userId);
                    await postRepository.DeleteAllFromUserAsync(userId);
                    break;
            }
            await viewHandler.GoToMainMenu();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}