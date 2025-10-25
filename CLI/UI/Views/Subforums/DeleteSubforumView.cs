using RepositoryContracts;

namespace CLI.UI.Views.Subforums;

public class DeleteSubforumView(
    ViewHandler viewHandler,
    ISubforumRepository subforumRepository,
    IPostRepository postRepository)
    : IView
{
    public void Display()
    {
        Console.WriteLine("-- Delete Subforum --");
        Console.WriteLine("To delete subforum, write id of subforum");
        Console.WriteLine("Example: [id]");
        Console.WriteLine("Type 'exit' to exit.");
        Console.WriteLine("---------------------");
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
                    int subforumId = int.Parse(input);
                    await subforumRepository.DeleteAsync(subforumId);
                    await postRepository.DeleteAllFromSubforumAsync(subforumId);
                    break;
            }

            await viewHandler.GoToMainMenu();
        }
        catch (Exception e)
        {
            await viewHandler.GoToView(Views.DeleteSubforum);
            Console.WriteLine(e.Message);
        }
    }
}