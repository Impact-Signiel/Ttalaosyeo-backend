
namespace signiel.Helpers;

public static class DayNightFormatter {
    public static string Format(uint days, uint nights) {
        if (days == 0 && nights == 0) {
            return "당일";
        }

        if (nights == 0) {
            return $"무박 {nights}일";
        }

        return $"{nights}박 {days}일";
    }
}