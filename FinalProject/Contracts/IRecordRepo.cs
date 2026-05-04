using FinalProject.Domain;

namespace FinalProject.Contracts;

public interface IRecordRepo
{
    public List<Record> GetAll();
    public List<Record> GetByBooking(int bookingId);
    public void Add(Record r);
}