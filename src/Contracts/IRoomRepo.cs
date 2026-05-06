using FinalProject.Domain;

namespace FinalProject.Contracts;

public interface IRoomRepo
{
    List<Room> GetAll();
    void AddRoom(Room r);
    Room? GetByLocation(string location);
    void DeleteRoom(Room r);
}