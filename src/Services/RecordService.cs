using FinalProject.Contracts;
using FinalProject.Domain;

namespace FinalProject.Services;

public class RecordService : IRecordService
{
    private readonly IRecordRepo _recordRepo;
    private readonly IBookingRepo _bookingRepo;

    public RecordService(IRecordRepo recordRepo,  IBookingRepo bookingRepo)
    {
        _recordRepo = recordRepo;
        _bookingRepo = bookingRepo;
    }

    public List<Record> GetAllRecords()
    {
        return _recordRepo.GetAll();
    }

    public Record? GetRecordById(int bookingId)
    {
        return _recordRepo.GetByBookingId(bookingId);
    }

    public void ArchiveBooking(Booking b)
    {
        if (_recordRepo.GetByBookingId(b.BookingId).HasValue) return;
        b.Status = BookingStatus.Cancelled;
        b.Room.Booked = false;
        b.Room.CurrentBooking = null;
        _recordRepo.Add(new Record(b));
        _bookingRepo.Delete(b);
    }
}