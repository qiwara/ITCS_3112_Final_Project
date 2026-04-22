namespace FinalProject.Contracts;

public interface IUserRepo
{
    public List<User> GetAllUsers(); 
    public Users GetUserByEmail(string email);
    public Users GetUserById(string username);
    public void AddUser(User u);
}
