using RepositoryContracts;

namespace CLI.UI.Views.Users;

public class DeleteUser(ViewHandler viewHandler, IUserRepository userRepository, IPostRepository postRepository)
    : IView
{
    private readonly ViewHandler _viewHandler = viewHandler;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPostRepository _postRepository = postRepository;

    public void Display()
    {
        Console.WriteLine("Delete User --> ");
        Console.WriteLine("Insert you'r userId");
        Console.WriteLine("");
        Console.WriteLine("Type '0' to exit.");
    }

    public void HandleInput(string input)
    {
        try
        {
            switch (input)
            {
                case "0":
                    break;
                default:
                    int userId = int.Parse(input);
                    _userRepository.DeleteAsync(userId);
                    _postRepository.DeleteAllFromUserAsync(userId);
                    break;
            }

            _viewHandler.GoToMainView();
        }
        catch (Exception e)
        {
            _viewHandler.GoToView(Views.DeleteUser);
        }
    }
}