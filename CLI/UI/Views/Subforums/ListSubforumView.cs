using RepositoryContracts;

namespace CLI.UI.Views.Subforums;

public class ListSubforumView(ViewHandler viewHandler, ISubforumRepository subforumRepository)
    : IView
{
    public void Display()
    {
        Console.WriteLine("-- Subforums listed --");
        Console.WriteLine("To open af subforum, type the ID of the subforum");
        Console.WriteLine("Example: [ID]");
        Console.WriteLine("Type 'exit' to exit.");
        Console.WriteLine("----------------------");

        var subforums = subforumRepository.GetMany();
        foreach (var subforum in subforums)
        {
            Console.WriteLine($"- Name: {subforum.Name} - Id: {subforum.SubforumId} - Mod Id: {subforum.ModeratorId}");
        }
    }

    public async Task HandleInput(string input)
    {
        try
        {
            switch (input.ToLower())
            {
                case "exit":
                    break;
                default:
                    int sfId = int.Parse(input);

                    var subforum = await subforumRepository.GetSingleAsync(sfId);
                    viewHandler.ViewState.CurrentSubforum = subforum;

                    await viewHandler.GoToView(Views.OpenSubforum);

                    break;
            }

            await viewHandler.GoToMainMenu();
        }
        catch (Exception e)
        {
            await viewHandler.GoToView(Views.ListSubforums);
            Console.WriteLine(e.Message);
        }
    }
}