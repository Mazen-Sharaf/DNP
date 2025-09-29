using CLI.UI;
using fileRepositories;
using FileRepositories;
using RepositoryContracts;

Console.WriteLine("Starting CLI app!");
IPostRepository posts = new PostFileRepository();
IUserRepository users = new UserFileRepository();
ISubforumRepository subforums = new SubforumFileRepository();

CLIApp app = new CLIApp(posts, users, subforums);
await app.StartAsync();