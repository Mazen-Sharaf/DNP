using Entities;
using RepositoryContracts;

namespace CLI.UI.Views.Subforums;

public class ManageSubforumView(ViewHandler viewHandler, ISubforumRepository subforumRepository)
    : IView
{
    public void Display()
    {
        Console.WriteLine("-- Manage Subforum --");
        Console.WriteLine("To change a subforum name, write id of subforum, command name and the new name");
        Console.WriteLine("Example: [sf id] name [New Name]");
        Console.WriteLine(
            "To change a subforum moderator, write the id of subforum, command mod and the id of the new moderator");
        Console.WriteLine("Example: [sf id] mod [new mod id]");
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
                    var splitInput = input.Split(" ");
                    var id = int.Parse(splitInput[0]);
                    var command = splitInput[1].ToLower();
                    if (command == "name")
                    {
                        int nameStartIndex = splitInput[0].Length + splitInput[1].Length + 2;
                        await subforumRepository.UpdateAsync(new Subforum()
                        {
                            SubforumId = id,
                            Name = input.Substring(nameStartIndex),
                            ModeratorId = (await subforumRepository.GetSingleAsync(id)).ModeratorId
                        });
                    }
                    else if (command == "mod")
                    {
                        await subforumRepository.UpdateAsync(new Subforum()
                        {
                            SubforumId = id,
                            Name = subforumRepository.GetSingleAsync(id).Result.Name,
                            ModeratorId = int.Parse(input.Split(" ")[3])
                        });
                    }

                    break;
            }

            await viewHandler.GoToMainMenu();
        }
        catch (Exception e)
        {
            await viewHandler.GoToView(Views.ManageSubforum);
            Console.WriteLine(e.Message);
        }
    }
}