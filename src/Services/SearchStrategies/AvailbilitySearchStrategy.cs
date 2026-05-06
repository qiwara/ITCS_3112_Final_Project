using FinalProject.Contracts;
using FinalProject.Domain;

namespace FinalProject.Services.SearchStrategies;

public class AvailabilitySearchStrategy : IBookingSearchStrategy
{
    public IEnumerable<Booking> Filter(IEnumerable<Booking> bookings) =>
        bookings.Where(b => b.Status == BookingStatus.Pending && 
                            b.Attendees.Count < b.Maximum);
}