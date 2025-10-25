using CLI.UI;
using FileRepositories;
using RepositoryContracts;

Console.WriteLine("Starting CLI app!");
IPostRepository posts = new PostFileRepository();
IUserRepository users = new UserFileRepository();
ISubforumRepository subforums = new SubforumFileRepository();
IReactionRepository reactions = new ReactionFileRepository();

CLIApp app = new CLIApp(posts, users, subforums, reactions);
await app.StartAsync();

Console.WriteLine("DONE !!");