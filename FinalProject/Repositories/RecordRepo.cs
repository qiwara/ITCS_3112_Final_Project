using FinalProject.Contracts;
using FinalProject.Domain;

namespace FinalProject.Repositories;

public class RecordRepo : IRecordRepo
{
    private readonly List<Record> _records = [];

    public List<Record> GetAll()
    {
        return _records;
    }

    public List<Record> GetByBooking(int bookingId)
    {
        return _records.Where(r => r.BookingId == bookingId).ToList();
    }

    public void Add(Record r)
    {
        _records.Add(r);
    }
}