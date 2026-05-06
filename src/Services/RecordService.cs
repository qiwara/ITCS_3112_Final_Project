using FinalProject.Contracts;
using FinalProject.Domain;

namespace FinalProject.Services;

public class RecordService : IRecordService
{
    private readonly IRecordRepo _recordRepo;

    public RecordService(IRecordRepo recordRepo)
    {
        _recordRepo = recordRepo;
    }

    public List<Record> GetAllRecords()
    {
        return _recordRepo.GetAll();
    }

    public List<Record> GetRecordsById(int bookingId)
    {
        return _recordRepo.GetByBookingId(bookingId);
    }

    public void ArchiveBooking(Booking b)
    {
        _recordRepo.Add(new Record(b));
    }
}