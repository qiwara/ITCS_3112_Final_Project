using System;
using System.Collections.Generic;

namespace FinalProject.Domain;

public class Booking
{
    public string Name { get; }
    public int BookingId { get; private set; }
    public Room Room { get; }
    public List<String> Attendees { get; } = new List<string>();
    public Subject Subject { get; }
    public int Minimum { get; }
    public int Maximum { get; }
    public DateTime ScheduledTime { get; }
    public DateTime Deadline { get; }
    public BookingStatus Status { get; set; }

    public Booking(
        string name,
        Room room,
        Subject subject,
        DateTime scheduledTime)
    {
        // can't schedule a booking in the past
        if (scheduledTime < DateTime.Now) 
            throw new ArgumentException("Scheduled time cannot be in the past.");
        
        Name = name;
        Room = room;
        Subject = subject;
        Minimum = (int)Math.Max(2, room.Capacity * 0.1);
        Maximum = Room.Capacity;
        ScheduledTime = scheduledTime;
        Status = BookingStatus.Pending; 
        
        // deadline is whichever comes sooner: 2 days from now OR the meeting
        if (DateTime.Now.AddDays(2) >= scheduledTime)
        {
            // Set deadline to the meeting time if less than 2 days until booking
            Deadline = scheduledTime; 
        }
        else
        {
            // otherwise, set deadline 2 days before booking
            Deadline = scheduledTime.AddDays(-2);
        }
    }
    
    public void AssignId(int id) => BookingId = id;
}