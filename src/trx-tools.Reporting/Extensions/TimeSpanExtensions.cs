namespace trx_tools.HtmlReporting.Extensions;

public static class TimeSpanExtensions
{
    public static string ToHumanReadableTimeSpan(this TimeSpan timeSpan)
    {
        var values = new List<string>();
        if (timeSpan.Days > 0)
        {
            values.Add($"{timeSpan.Days}d");
        }

        if (timeSpan.Hours > 0)
        {
            values.Add($"{timeSpan.Hours}h");
        }

        if (timeSpan.Minutes > 0)
        {
            values.Add($"{timeSpan.Minutes}m");
        }

        if (timeSpan.Seconds > 0)
        {
            values.Add($"{timeSpan.Seconds}s");
        }

        if (timeSpan.Milliseconds > 0)
            values.Add($"{timeSpan.Milliseconds}ms");
        else if (values.Count == 0)
            values.Add("< 1ms");

        return string.Join(" ", values);
    }
}