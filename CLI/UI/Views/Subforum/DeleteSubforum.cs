using Entities;
using RepositoryContracts;
namespace CLI.UI.Views.Subforum;

public class DeleteSubforum: IView
{
    private readonly ViewHandler _viewHandler;
    private readonly ISubforumRepository _subforumRepository;
    private readonly IPostRepository _postRepository;
    
    public DeleteSubforum(ViewHandler viewHandler, ISubforumRepository subforumRepository, IPostRepository postRepository)
    {
        _viewHandler = viewHandler;
        _subforumRepository = subforumRepository;
        _postRepository = postRepository;
    }
    public void Display()
    {
        Console.WriteLine("Delete Subforum --> ");
        Console.WriteLine("To delete subforum, write id of subforum");
        Console.WriteLine("Type '0' to exit.");
        Console.WriteLine("---------------------");
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
                    int subforumId = int.Parse(input);
                    _subforumRepository.DeleteAsync(subforumId);
                    _postRepository.DeleteAllFromSubforumAsync(subforumId);
                    break;
            }
            _viewHandler.GoToMainView();
        }
        catch (Exception e)
        {
            _viewHandler.GoToView(Views.DeleteSubforum);
        }
    }
}