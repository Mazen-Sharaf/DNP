using CLI.UI;
using InMemoryRepositories;
using RepositoryContracts;

Console.WriteLine("Starting CLI app!");
IPostRepository posts = new PostInMemoryRepository();
IUserRepository users = new UserInMemoryRepository();
ISubforumRepository subforums = new SubforumMemoryRepository();

CLIApp app = new CLIApp(posts, users, subforums);
await app.StartAsync();