using FinalProject.Contracts;

namespace FinalProject.Repositories;

public class UserRepo : IUserRepo
{
    private List<Users>  _users = new();

    public UserRepo()
    {
        //Empty Constructr
    }
    
    public List<User> GetAllUsers()
    {
        return  _users;
    }

    public Users GetUserByEmail(string email)
    {
        List<Users> returnList = new();
        foreach (User user in _users)
        {
            if (user.getEmail().Equals(email))
            {
                returnList.add(user);
            }
        }
        reuturn returnList;
    }

    public Users GetUserById(string username)
    {
        List<Users> returnList = new();
        foreach (User user in _users)
        {
            if (user.name.Equals(username))
            {
                returnList.add(user);
            }
        }
        reuturn returnList;
    }

    public void AddUser(User u)
    {
        if (!_users.Contains(u))
        {
            _users.add(u);
        }
    }
}