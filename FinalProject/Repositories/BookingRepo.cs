using System.Collections.Generic;
using FinalProject.Contracts;
using FinalProject.Domain;

namespace FinalProject.Repositories;
public class BookingRepo : IBookingRepo
{
    private readonly List<Booking> _bookings = [];

    public List<Booking> GetAll()
    {
        return _bookings;
    }

    public void Add(Booking b)
    {
        _bookings.Add(b);
    }

    public void Delete(Booking b)
    {
        _bookings.Remove(b);
    }

    public List<Booking> GetBookingsByRoom(Room r)
    {
        return _bookings.FindAll(b => b.Room == r);
    }
}