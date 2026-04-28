namespace FinalProject.Domain;

public abstract class User
{
    public string name;
    protected string email;
    protected string password;
    protected User(string name, string email, string password)
    {
        this.name = name;
        this.email = email;
        this.password = password;
    }

    public string getEmail()
    {
        return email;
    }

    public string getPassword()
    {
        return password;
    }
    
    public bool checkPassword(String attempt)
    {
        return password.Equals(attempt);
    }
}