using System.Runtime.InteropServices;
using CLI.UI.Views;
using CLI.UI.Views.Users;
using Entities;
using RepositoryContracts;

namespace CLI.UI
{
    public class CLIApp
    {
        private IPostRepository _posts;
        private IUserRepository _users;
        private ISubforumRepository _subforums;

        private ViewHandler _viewHandler;

        public CLIApp(IPostRepository posts, IUserRepository users, ISubforumRepository subforums)
        {
            _posts = posts;
            _users = users;
            _subforums = subforums;
            

            _viewHandler = new ViewHandler(posts, users, subforums);
        }

        public Task StartAsync()
        {
            _viewHandler.GoToMainView();

            return Task.CompletedTask;
        }
    }
}