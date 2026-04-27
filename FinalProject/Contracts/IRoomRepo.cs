using FinalProject.Domain;

namespace FinalProject.Contracts;

public interface IRoomRepo
{
    List<Room> GetAll();
    void AddRoom(Room r);
    Room GetById(int id);
    void DeleteRoom(Room r);
}