using System.Collections.Generic;
using FinalProject.Contracts;
using FinalProject.Domain;

namespace FinalProject.Services;

public class BookingService : IBookingService
{
    private IBookingRepo _bookingRepo;

    public BookingService(IBookingRepo repo)
    {
        _bookingRepo = repo;
    }
    public void AddBooking(Booking b)
    {
        _bookingRepo.Add(b);
    }

    public void DeleteBooking(Booking b)
    {
        _bookingRepo.Delete(b);
    }

    public List<Booking> GetAllBookings()
    {
        return _bookingRepo.GetAll();
    }

    public List<Booking> GetBookingsByRoom(Room r)
    {
        return _bookingRepo.GetBookingsByRoom(r);
    }

    public bool IsRoomAvailable(Room r)
    {
        return !r.Booked;
    }
}