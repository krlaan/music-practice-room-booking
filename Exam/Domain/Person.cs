using BLL;

namespace Domain;

public class Person : BaseEntity
{
    public string FullName { get; set; } = default!;

    public EUserType UserType { get; set; } = EUserType.Student;
    
    // For students only
    public EProgramType? ProgramType { get; set; }
    
    // Weekly quota in hours (for students)
    public int? WeeklyQuotaHours { get; set; }
    
    // Current week used hours (for students)
    public double CurrentWeekUsedHours { get; set; }
    
    // Week start date for quota tracking (for students)
    public DateTime? WeekStartDate { get; set; }
    
    // Total practice hours for graduation (for students)
    public double TotalPracticeHours { get; set; }
    
    // No-show tracking (for students)
    public int NoShowCount { get; set; }
    
    // Penalty multiplier (e.g., 2.0 means -2 hours per no-show)
    public double PenaltyMultiplier { get; set; } = 2.0;
    
    // Calculated penalty hours
    public double NoShowPenaltyHours => NoShowCount * PenaltyMultiplier;
    
    // Navigation property
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
