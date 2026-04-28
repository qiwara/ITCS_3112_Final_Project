using FinalProject.Contracts;

namespace FinalProject.Services;

public class LoginService : ILoginService
{
    private IUserRepo _repo;

    public LoginService(IUserRepo repo)
    {
        _repo = repo;
    }
    
    public User Login(string userName, string password)
    {
        User user = _repo.GetUserById(userName);
        if (user != null && user.checkPassword(password))
        {
            return user;
        }
        else 
        {
            return null;
        }
    }
    
    public void Logout()
    {}
    // these r kinda not needed. program can know if logged in with the user var
    public bool IsLoggedIn()
    {}
}