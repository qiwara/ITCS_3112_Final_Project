using FinalProject.Contracts;
using FinalProject.Domain;

namespace FinalProject.Repositories;

public class RoomRepo : IRoomRepo
{
    private readonly List<Room> roomList = [];

    public List<Room> GetAll()
    {
        return roomList;
    }

    public void AddRoom(Room r)
    {
        roomList.Add(r);
    }

    public Room GetById(int id)
    {
        return roomList[id];
    }

    public void DeleteRoom(Room r)
    {
        roomList.Remove(r);
    }
}