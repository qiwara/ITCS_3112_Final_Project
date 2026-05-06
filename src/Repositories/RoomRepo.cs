using FinalProject.Contracts;
using FinalProject.Domain;

namespace FinalProject.Repositories;

public class RoomRepo : IRoomRepo
{
    private readonly List<Room> _roomList = [];

    public List<Room> GetAll()
    {
        return _roomList;
    }

    public void AddRoom(Room r)
    {
        _roomList.Add(r);
    }

    public Room? GetByLocation(string location)
    {
        return _roomList.FirstOrDefault(r => r.Location == location);
    }

    public void DeleteRoom(Room r)
    {
        _roomList.Remove(r);
    }
}