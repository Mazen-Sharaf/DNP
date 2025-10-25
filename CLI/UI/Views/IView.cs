namespace CLI.UI.Views;

public interface IView
{
    public void Display();
    public Task HandleInput(string input);
}