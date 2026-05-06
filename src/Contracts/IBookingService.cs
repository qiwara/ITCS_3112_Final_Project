using System.Collections.Generic;
using FinalProject.Domain;

namespace FinalProject.Contracts;

public interface IBookingService
{
    public Booking CreateBooking(string sessionName, string location, Subject subject, DateTime scheduledTime, string email);
    public void AddBooking(Booking b);
    public List<Booking> GetAllBookings();
    public List<Booking> SearchBookings(IBookingSearchStrategy strategy);
    public void JoinBooking(int bookingId, string email);
    public void LeaveBooking(int bookingId, string email);
}