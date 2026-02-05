using BLL;

namespace Domain;

public class Booking : BaseEntity
{
    // Booking times
    public DateTime StartTime { get; set; }
    
    public DateTime EndTime { get; set; }
    
    // Status
    public EBookingStatus Status { get; set; } = EBookingStatus.Confirmed;
    
    // Approval workflow
    public bool RequiresApproval { get; set; }
    
    public bool IsRecitalPrep { get; set; }
    
    // Calculated property for booking duration in hours
    public double DurationHours => (EndTime - StartTime).TotalHours;
    
    // Foreign keys
    public Guid StudentId { get; set; }
    public Person Student { get; set; } = default!;
        
    public Guid RoomId { get; set; }
    public Room Room { get; set; } = default!;
}
