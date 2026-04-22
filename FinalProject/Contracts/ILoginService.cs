namespace FinalProject.Contracts;

public interface ILoginService
{
    public User Login(string username, string password);
    public void Logout();
    public bool IsLoggedIn();
}