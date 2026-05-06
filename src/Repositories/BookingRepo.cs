using System.Collections.Generic;
using FinalProject.Contracts;
using FinalProject.Domain;

namespace FinalProject.Repositories;
public class BookingRepo : IBookingRepo
{
    private readonly List<Booking> _bookings = new List<Booking>();
    private int _nextId = 1;

    public List<Booking> GetAll()
    {
        return _bookings;
    }

    public void Add(Booking b)
    {
        b.AssignId(_nextId);
        _nextId++;
        _bookings.Add(b);
    }

    public void Delete(Booking b)
    {
        _bookings.Remove(b);
    }

    public Booking? GetById(int id)
    {
        return _bookings.FirstOrDefault(b => b.BookingId == id);
    }
}