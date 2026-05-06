namespace FinalProject.Domain;

public readonly struct Record
{
    public int BookingId { get; }
    public string RoomLocation { get; }
    public BookingStatus Status { get; }
    public DateTime? ScheduledTime { get; }
    public DateTime ArchivedTime { get; }

    public Record(Booking booking)
    {
        this.BookingId = booking.BookingId;
        this.RoomLocation = booking.Room.Location;
        this.Status = booking.Status;
        this.ScheduledTime = booking.ScheduledTime;
        this.ArchivedTime = DateTime.Now;
    }
}
