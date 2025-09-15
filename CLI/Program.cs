using CLI.UI;
using InMemoryRepositories;
using RepositoryContracts;

Console.WriteLine("Starting CLI app!");
IPostRepository posts = new PostInMemoryRepository();
IUserRepository users = new UserInMemoryRepository();

CLIApp app = new CLIApp(posts, users);
await app.StartAsync();