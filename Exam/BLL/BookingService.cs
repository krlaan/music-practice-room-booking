namespace BLL;

public static class BookingService
{
    // Jury exam date (hardcoded for this semester)
    public static readonly DateTime JuryExamDate = new DateTime(2026, 2, 10);
    
    // Get maximum booking duration based on whether it's recital prep
    public static double GetMaxBookingDuration(bool isRecitalPrep)
    {
        return isRecitalPrep ? 3 : 2;
    }
    
    // Check if current date is within 2 weeks before jury exams
    public static bool IsWithinRecitalPrepPeriod(DateTime bookingDate)
    {
        var daysUntilExam = (JuryExamDate - bookingDate.Date).TotalDays;
        return daysUntilExam >= 0 && daysUntilExam <= 14;
    }
    
    // Validate booking duration
    public static (bool isValid, string? errorMessage) ValidateBookingDuration(double durationHours, bool isRecitalPrep)
    {
        var maxDuration = GetMaxBookingDuration(isRecitalPrep);
        
        if (durationHours > maxDuration)
        {
            return (false, $"Maximum booking duration is {maxDuration} hours per booking.");
        }
        
        return (true, null);
    }
    
    // Validate if student has enough quota
    public static (bool isValid, string? errorMessage) ValidateQuota(
        double durationHours, 
        double availableHours)
    {
        if (durationHours > availableHours)
        {
            return (false, $"Insufficient weekly quota. Available: {availableHours} hours, Requested: {durationHours} hours.");
        }
        
        return (true, null);
    }
}
