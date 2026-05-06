using FinalProject.Domain;

namespace FinalProject.Contracts;

public interface IUserRepo
{
    public List<User> GetAllUsers();
    public User? GetUserByEmail(string email);
    public User? GetUserById(string username);
    public void AddUser(User u);
}
