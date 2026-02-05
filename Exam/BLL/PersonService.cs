namespace BLL;

public static class PersonService
{
    // Get weekly quota hours based on program type
    public static double? GetWeeklyQuotaForProgram(EProgramType? programType)
    {
        if (!programType.HasValue)
            return null;
            
        return programType.Value switch
        {
            EProgramType.Performance => 20,
            EProgramType.Education => 10,
            EProgramType.Minor => 5,
            _ => 0
        };
    }
    
    // Calculate available hours for a student (quota - used - penalties)
    public static double CalculateAvailableHours(double? weeklyQuota, double currentWeekUsed, double penaltyHours)
    {
        return (weeklyQuota ?? 0) - currentWeekUsed - penaltyHours;
    }
}
