namespace FinalProject.Domain;

public struct Record
{
    public int BookingId;
    public int UserId;
    public DateTime TimeStamp;

    public Record(int bookingId, int userId, DateTime timeStamp)
    {
        BookingId = bookingId;
        UserId = userId;
        TimeStamp = timeStamp;
    }
}
