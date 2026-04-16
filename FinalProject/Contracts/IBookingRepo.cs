using System.Collections.Generic;
using FinalProject.Domain;

namespace FinalProject.Contracts;

public interface IBookingRepo
{
    public List<Booking> GetAll();
    public void Add(Booking b);
    public void Delete(Booking b);
    public List<Booking> GetBookingByRoom(Room r);
}