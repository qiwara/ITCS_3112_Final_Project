using FinalProject.Domain;

namespace FinalProject.Contracts;

public interface IRecordRepo
{
    public List<Record> GetAll();
    public Record? GetByBookingId(int bookingId);
    public void Add(Record r);
}