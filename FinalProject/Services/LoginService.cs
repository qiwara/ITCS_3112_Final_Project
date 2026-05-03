using FinalProject.Contracts;
using FinalProject.Domain;

namespace FinalProject.Services;

public class LoginService : ILoginService
{
    private IUserRepo _userRepo;

    public LoginService(IUserRepo repo)
    {
        _userRepo = repo;
    }

    public User Login(string email, string pw)
    {
        User user = _userRepo.GetUserByEmail(email);
        if (user != null && user.checkPassword(pw))
        {
            return user;
        }
        return null;
    }

    public void Logout(Session session)
    {
        session.Invalidate();
    }

    public bool IsLoggedIn(Session session)
    {
        return session != null && session.IsActive;
    }
}
