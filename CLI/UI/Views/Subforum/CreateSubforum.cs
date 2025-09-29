using Entities;
using InMemoryRepositories;
using RepositoryContracts;

namespace CLI.UI.Views.Subforum;

public class CreateSubforum(
    ViewHandler viewHandler,
    ISubforumRepository subforumRepository,
    IUserRepository userRepository) : IView
{
    private readonly ViewHandler _viewHandler = viewHandler;
    private readonly ISubforumRepository _subforumRepository = subforumRepository;
    private readonly IUserRepository _userRepository = userRepository;

    public void Display()
    {
        Console.WriteLine("Create Subforum -->");
        Console.WriteLine("Enter the id of the Moderator and a name of subforum");
        Console.WriteLine("Type '0' to exit");
    }

    public void HandleInput(string input)
    {
        try
        {
            switch (input)
            {
                case "0":
                    _viewHandler.GoToMainView();
                    break;
                default:
                    var args = input.Split(" ");
                    var moderatorId = int.Parse(args[0]);
                    var name = input.Substring(args[0].Length + 1);

                    if (_subforumRepository.GetMany().Any(s => s.Name.ToLower() == name.ToLower()))
                        break;
                    if (!_userRepository.GetMany().Any(u => u.UserId == moderatorId))
                        break;

                    _subforumRepository.AddAsync(new Entities.Subforum()
                    {
                        ModeratorId = moderatorId,
                        Name = name
                    });
                    break;
            }

            _viewHandler.GoToMainView();
        }
        catch (Exception e)
        {
            _viewHandler.GoToView(Views.CreateSubforum);
        }
    }
}