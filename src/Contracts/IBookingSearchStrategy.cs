using FinalProject.Domain;

namespace FinalProject.Contracts;

public interface IBookingSearchStrategy
{
    
    IEnumerable<Booking> Filter(IEnumerable<Booking> bookings);
}