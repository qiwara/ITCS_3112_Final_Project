using FinalProject.Contracts;
using FinalProject.Domain;

namespace FinalProject.Services.SearchStrategies;

public class RoomSearchStrategy : IBookingSearchStrategy
{
    private readonly string _room;

    public RoomSearchStrategy(string room) => _room = room;

    public IEnumerable<Booking> Filter(IEnumerable<Booking> bookings) =>
        bookings.Where(b => b.Room.Location.Equals(_room, StringComparison.OrdinalIgnoreCase));
}
