namespace FinalProject.Domain;

public class RoomFactory
{
    public Room CreateRoom(string loc, string cap, string type)
    {
        int capacity = int.Parse(cap);
        return type.ToLower() switch
        {
            "classroom" => new Classroom(loc, capacity),
            "lab"       => new Lab(loc, capacity),
            "studyroom" => new StudyRoom(loc, capacity),
            _ => throw new ArgumentException($"Unknown room type: {type}")
        };
    }
}
