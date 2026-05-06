using FinalProject.Contracts;
using FinalProject.Domain;

namespace FinalProject.Services.SearchStrategies;

public class SubjectSearchStrategy : IBookingSearchStrategy
{
    private readonly Subject _subject;
    
    public SubjectSearchStrategy(Subject subject) => _subject = subject;

    public IEnumerable<Booking> Filter(IEnumerable<Booking> bookings) =>
        bookings.Where(b => b.Subject == _subject);
}