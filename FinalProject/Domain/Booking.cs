using System;
using System.Collections.Generic;

namespace FinalProject.Domain;

public class Booking
{
    public Room Room { get; set; }
    public List<String> Attendees { get; set; }
    public Subject Subject { get; set; }
    public int Minimum { get; set; }
    public int Maximum { get; set; }
    public DateTime CreatedTime { get; set; }
    public DateTime Deadline { get; set; }
    public BookingStatus Status { get; set; }

    public Booking(
        Room room,
        List<string> attendeeList,
        Subject subject,
        int min,
        int max,
        DateTime createdTime,
        DateTime deadline,
        BookingStatus status)
    {
        Room = room;
        Attendees = attendeeList;
        Subject = subject;
        Minimum = min;
        Maximum = max;
        CreatedTime = createdTime;
        Deadline = deadline;
        Status = status;
    }
}