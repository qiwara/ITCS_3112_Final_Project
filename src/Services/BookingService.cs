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
        if (room == null) throw new ArgumentException("Room location not found.");
        if (room.Booked) throw new InvalidOperationException("Room is already booked.");
        
        var newBooking = new Booking(sessionName, room, subject, scheduledTime);
        newBooking.Attendees.Add(email);

        room.Booked = true;
        room.CurrentBooking = newBooking;
        
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
        if (booking == null) throw new ArgumentException("Booking not found.");
        if (booking.Status == BookingStatus.Cancelled || booking.Status == BookingStatus.Expired)
            throw new InvalidOperationException("Cannot join an inactive session.");
        
        string userStr = email;
        if (booking.Attendees.Contains(userStr)) return;
        if (booking.Attendees.Count >= booking.Maximum) throw new InvalidOperationException("Session is full.");

        booking.Attendees.Add(userStr);

        if (booking.Status == BookingStatus.Pending && booking.Attendees.Count >= booking.Minimum)
        {
            booking.Status = BookingStatus.Confirmed;
        }
    }

    public void LeaveBooking(int bookingId, string email)
    {
        var booking = _bookingRepo.GetById(bookingId);
        if (booking == null) throw new ArgumentException("Booking not found.");
        if (booking.Status == BookingStatus.Cancelled || booking.Status == BookingStatus.Expired)
            throw new InvalidOperationException("Cannot leave an inactive session.");
        
        booking.Attendees.Remove(email);

        if (booking.Attendees.Count < booking.Minimum && booking.Status == BookingStatus.Confirmed)
        {
            booking.Status = BookingStatus.Pending;
        }
    }
}