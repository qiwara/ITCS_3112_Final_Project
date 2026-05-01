using FinalProject.Contracts;

namespace FinalProject.Repositories;

public class UserRepo : IUserRepo
{
    private List<Users>  _users = new();

    public UserRepo()
    {
        //Empty Constructor
    }
    
    public List<User> GetAllUsers()
    {
        return  _users;
    }

    public Users GetUserByEmail(string email)
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

    public Users GetUserById(string username)
    {
        foreach (User user in _users)
        {
            if (user.name.Equals(username))
            {
                return user
            }
        }
        return  null;
    }

    public void AddUser(User u)
    {
        if (!_users.Contains(u))
        {
            _users.add(u);
        }
    }
}
