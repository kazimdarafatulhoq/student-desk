namespace StudentManagement.Application.Security;

public static class AgeCalculator
{
    public static (int Years, int Months) Calculate(DateTime dateOfBirth, DateTime? asOf = null)
    {
        var today = asOf?.Date ?? DateTime.Today;
        if (dateOfBirth.Date > today)
            return (0, 0);

        var years = today.Year - dateOfBirth.Year;
        var months = today.Month - dateOfBirth.Month;

        if (today.Day < dateOfBirth.Day)
            months--;

        if (months < 0)
        {
            years--;
            months += 12;
        }

        return (years, months);
    }

    public static string Format(DateTime dateOfBirth, DateTime? asOf = null)
    {
        var (years, months) = Calculate(dateOfBirth, asOf);
        return $"{years} Yr {months} Mo";
    }
}
