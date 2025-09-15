namespace CLI.UI.Views;

public interface IView
{
    public void Display();
    public void HandleInput(string input);
}