using BLL;

namespace Domain;

public class Room : BaseEntity
{
    public string RoomNumber { get; set; } = default!;
    
    public ERoomSize Size { get; set; }
    
    // Equipment flags
    public bool HasGrandPiano { get; set; }
    
    public bool HasUprightPiano { get; set; }
    
    public bool IsSoundproofed { get; set; }
    
    // Navigation property
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
