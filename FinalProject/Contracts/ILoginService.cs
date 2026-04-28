namespace FinalProject.Contracts;

public interface ILoginService
{
    /// <summary>
    /// takes the username and password to return if a user matches those values. if none then returns null
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns>User that credentials match or null if none</returns>
    public User Login(string username, string password);
    public void Logout();
    public bool IsLoggedIn();
}