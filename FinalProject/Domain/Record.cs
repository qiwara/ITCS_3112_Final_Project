namespace FinalProject.Domain;

public struct Record
{
    public int BookingId { get; }
    public string RoomLocation { get; }
    public BookingStatus Status;
    public DateTime? ScheduledTime;
    public DateTime ArchivedTime;

    public Record(Booking booking)
    {
        this.BookingId = booking.BookingId;
        this.RoomLocation = booking.Room.Location;
        this.Status = booking.Status;
        this.ScheduledTime = booking.ScheduledTime;
        this.ArchivedTime = DateTime.Now;
    }
}
