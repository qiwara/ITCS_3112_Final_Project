namespace FinalProject.Domain;

public class StudyRoom : Room
{
    public StudyRoom(string loc, int cap) : base(loc, cap) { }

    public override void DisplayDetails()
    {
        Console.WriteLine($"Study Room | Location: {Location} | Capacity: {Capacity} | Booked: {Booked}");
    }
}
