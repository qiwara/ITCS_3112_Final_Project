using FinalProject.Contracts;

namespace FinalProject.Services;

public class AdminInterface : IUserInterface
{
    Dictionary<int, string> options = new Dictionary<int, string>
    {
        { 1, "Book" },
        { 2, "Bookings" },
        { 3, "Rooms" },
        { 4, "Sign Out" }
    };

    public string GetOptions()
    {
        return "1. Add booking \n" +
               "2. Look for booking \n" +
               "3. Look at rooms \n" +
               "4. Sign out";
    } // TODO add other options

    public string ChooseOption(int option)
    {
        return options[option];
    }
}
