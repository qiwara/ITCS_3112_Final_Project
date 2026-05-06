using FinalProject.Domain;

namespace FinalProject.Contracts;

public interface ILoginService
{
    public User? Login(string email, string pw);
    public void Logout(Session session);
    public bool IsLoggedIn(Session session);
}
