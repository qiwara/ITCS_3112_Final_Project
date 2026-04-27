namespace FinalProject.Domain;

public abstract class Room
{
    public string Location { get; set; }
    public int Capacity { get; set; }
    public bool Booked { get; set; }
    public Booking? CurrentBooking { get; set; }

    protected Room(string location, int capacity)
    {
        Location = location;
        Capacity = capacity;
        Booked = false;
        CurrentBooking = null;
    }

    public abstract void DisplayDetails();
}