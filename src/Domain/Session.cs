namespace FinalProject.Domain;

public class Session
{
    public User CurrentUser { get; private set; }
    public bool IsActive { get; private set; }

    public Session(User user)
    {
        CurrentUser = user;
        IsActive = true;
    }

    public void Invalidate()
    {
        IsActive = false;
        CurrentUser = null;
    }
}
