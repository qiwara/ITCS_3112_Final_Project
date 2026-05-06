using System.Collections.Generic;
using FinalProject.Domain;

namespace FinalProject.Contracts;

public interface IBookingService
{
    public void AddBooking(Booking b);
    public void DeleteBooking(Booking b);
    public List<Booking> GetAllBookings();
    public List<Booking> GetBookingsByRoom(Room r);
    public bool IsRoomAvailable(Room r);
}