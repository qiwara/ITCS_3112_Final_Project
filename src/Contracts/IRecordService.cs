using FinalProject.Domain;

namespace FinalProject.Contracts;

public interface IRecordService
{
    public List<Record> GetAllRecords();
    public Record? GetRecordById(int bookingId);
    public void ArchiveBooking(Booking booking);
}