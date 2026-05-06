using FinalProject.Contracts;
using FinalProject.Domain;

namespace FinalProject.Services.SearchStrategies;

public class RoomSearchStrategy : IBookingSearchStrategy
{
    private readonly string _roomType;

    public RoomSearchStrategy(string roomType) => _roomType = roomType;

    public IEnumerable<Booking> Filter(IEnumerable<Booking> bookings) =>
        bookings.Where(b => b.Room.GetType().Name.Equals(_roomType, StringComparison.OrdinalIgnoreCase));
}
