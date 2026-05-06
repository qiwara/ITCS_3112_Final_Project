namespace FinalProject.Domain;

public class Lab : Room
{
    public string Equipments { get; set; }

    public Lab(string loc, int cap) : base(loc, cap)
    {
        Equipments = string.Empty;
    }

    public override void DisplayDetails()
    {
        Console.WriteLine($"Lab | Location: {Location} | Capacity: {Capacity} | Equipment: {Equipments} | Booked: {Booked}");
    }
}
