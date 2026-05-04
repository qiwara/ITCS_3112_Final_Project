using FinalProject.Contracts;
using FinalProject.Domain;

namespace FinalProject.Services;

public class RecordService : IRecordService
{
    private IRecordRepo _recordRepo;

    public RecordService(IRecordRepo recordRepo)
    {
        _recordRepo = recordRepo;
    }

    public List<Record> GetAllRecords()
    {
        return  _recordRepo.GetAll();
    }

    public List<Record> GetRecordsById(int bookingId)
    {
        return _recordRepo.GetAll().Where(x => x.BookingId == bookingId).ToList();
    }

    public void AddRecord(Record r)
    {
        _recordRepo.Add(r);
    }
}