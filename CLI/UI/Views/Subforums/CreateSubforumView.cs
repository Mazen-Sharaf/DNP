using Entities;
using RepositoryContracts;

namespace CLI.UI.Views.Subforums;

public class CreateSubforumView(
    ViewHandler viewHandler,
    ISubforumRepository subforumRepository,
    IUserRepository userRepository)
    : IView
{
    public void Display()
    {
        Console.WriteLine("-- Create Subforum --");
        Console.WriteLine("To create a new subforum, type id of moderator and name of subforum");
        Console.WriteLine("Example: [id] [name]");
        Console.WriteLine("Type 'exit' to exit.");
        Console.WriteLine("---------------------");
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
                    var args = input.Split(" ");
                    var moderatorId = int.Parse(args[0]);
                    var name = input.Substring(args[0].Length + 1);

                    // Hvis der allerede er et subforum med dette navn
                    if (subforumRepository.GetMany().Any(s => s.Name.ToLower() == name.ToLower()))
                        break;
                    // Hvis brugeren ikke findes
                    if (!userRepository.GetMany().Any(u => u.UserId == moderatorId))
                        break;

                    await subforumRepository.AddAsync(new Subforum()
                    {
                        ModeratorId = moderatorId,
                        Name = name
                    });

                    break;
            }

            await viewHandler.GoToMainMenu();
        }
        catch (Exception e)
        {
            await viewHandler.GoToView(Views.CreateSubforum);
            Console.WriteLine(e.Message);
        }
    }
}