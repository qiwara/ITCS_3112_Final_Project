using System.Collections.Generic;
using FinalProject.Contracts;
using FinalProject.Domain;

namespace FinalProject.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepo _bookingRepo;
    private readonly IRoomRepo _roomRepo;

    public BookingService(IBookingRepo repo, IRoomRepo roomRepo)
    {
        _bookingRepo = repo;
        _roomRepo = roomRepo;
    }
    
    public Booking CreateBooking(string sessionName, string location, Subject subject, DateTime scheduledTime, string email)
    {
        var room = _roomRepo.GetByLocation(location);
        if (room == null) throw new Exception("Room location not found.");

        var newBooking = new Booking(sessionName, room, subject, scheduledTime);
        
        newBooking.Attendees.Add(email);

        _bookingRepo.Add(newBooking);
        return newBooking;
    }
    
    public void AddBooking(Booking b) => _bookingRepo.Add(b);
    
    public List<Booking> GetAllBookings() => _bookingRepo.GetAll();

    public List<Booking> SearchBookings(IBookingSearchStrategy strategy)
    {
        var allBookings = _bookingRepo.GetAll();
        return strategy.Filter(allBookings).ToList();
    }

    public void JoinBooking(int bookingId, string email)
    {
        var booking = _bookingRepo.GetById(bookingId);
        if (booking == null) throw new Exception("Booking not found.");
        
        string userStr = email;
        if (booking.Attendees.Contains(userStr)) return;
        if (booking.Attendees.Count >= booking.Maximum) throw new Exception("Session is full.");

        booking.Attendees.Add(userStr);

        if (booking.Status == BookingStatus.Pending && booking.Attendees.Count >= booking.Minimum)
        {
            booking.Status = BookingStatus.Confirmed;
        }
    }

    public void LeaveBooking(int bookingId, string email)
    {
        var booking = _bookingRepo.GetById(bookingId);
        
        booking.Attendees.Remove(email);

        if (booking.Attendees.Count < booking.Minimum && booking.Status == BookingStatus.Confirmed)
        {
            booking.Status = BookingStatus.Pending;
        }
    }
}