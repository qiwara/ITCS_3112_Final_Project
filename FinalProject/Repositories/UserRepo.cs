using FinalProject.Contracts;
using FinalProject.Domain;

namespace FinalProject.Repositories;

public class UserRepo : IUserRepo
{
    private List<User> _users = new();

    public UserRepo()
    {
        //Empty Constructor
    }

    public List<User> GetAllUsers()
    {
        return _users;
    }

    public User? GetUserByEmail(string email)
    {
        foreach (User user in _users)
        {
            if (user.getEmail().Equals(email))
            {
                return user;
            }
        }
        return null;
    }

    public User? GetUserById(string username)
    {
        foreach (User user in _users)
        {
            if (user.name.Equals(username))
            {
                return user;
            }
        }
        return null;
    }

    public void AddUser(User u)
    {
        if (!_users.Contains(u))
        {
            _users.Add(u);
        }
    }
}
