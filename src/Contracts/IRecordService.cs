using FinalProject.Domain;

namespace FinalProject.Contracts;

public interface IRecordService
{
    public List<Record> GetAllRecords();
    public List<Record> GetRecordsById(int bookingId);
    public void ArchiveBooking(Booking booking);
}