namespace FinalProject.Domain;

public class Classroom : Room
{
    public int NumberOfWhiteBoards { get; set; }

    public Classroom(string loc, int cap) : base(loc, cap) { }

    public override void DisplayDetails()
    {
        Console.WriteLine($"Classroom | Location: {Location} | Capacity: {Capacity} | Whiteboards: {NumberOfWhiteBoards} | Booked: {Booked}");
    }
}
