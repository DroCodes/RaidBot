using System.Globalization;
using DSharpPlus.Entities;
using TimeZoneConverter;

namespace RaidBot.Util;

public class FormatDateTime
{
    public DateTime ParseDateTime(string date)
    {
        string[] formats = { "MM/dd/ HH:mm", "MM/dd/ HHmm", "MM-dd HH:mm", "MM-dd HHmm" };
    
        // Attempt to parse the input string as a date with a time
        if (DateTime.TryParseExact(date, formats, CultureInfo.InvariantCulture, DateTimeStyles.None,
                out DateTime parsedDateTime))
        {
            // Specify that the parsed date/time is in Eastern time
            var easternTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
    
            // Convert the parsed date/time to UTC
            var utcDateTime = TimeZoneInfo.ConvertTimeToUtc(parsedDateTime, easternTimeZone);
    
            // Return the converted date/time with a UTC DateTimeKind
            return utcDateTime;
        }
    
        // Attempt to parse the input string as a date without a time
        if (DateTime.TryParseExact(date, formats, CultureInfo.InvariantCulture, DateTimeStyles.None,
                out DateTime parsedDate))
        {
            // Combine the parsed date with the current time to create a DateTime object with a time component
            DateTime currentDate = DateTime.Now;
            parsedDateTime = new DateTime(currentDate.Year, parsedDate.Month, parsedDate.Day, currentDate.Hour,
                currentDate.Minute, currentDate.Second);
    
            // Specify that the parsed date/time is in Eastern time
            var easternTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
    
            // Convert the parsed date/time to UTC
            var utcDateTime = TimeZoneInfo.ConvertTimeToUtc(parsedDateTime, easternTimeZone);
    
            // Return the converted date/time with a UTC DateTimeKind
            return utcDateTime;
        }
    
        // If the input string could not be parsed as a date, throw an exception
        throw new ArgumentException("Invalid date format", nameof(date));
    }
    
    public long ParseUnixTime(string date)
{
    string[] formats = { "MM/dd/ HH:mm", "MM/dd/ HHmm", "MM-dd HH:mm", "MM-dd HHmm", "dddd, MMMM dd, yyyy h:mm tt" };

    // Attempt to parse the input string as a date with a time
    if (DateTime.TryParseExact(date, formats, CultureInfo.InvariantCulture, DateTimeStyles.None,
            out DateTime parsedDateTime))
    {
        // Specify that the parsed date/time is in Eastern time
        var easternTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

        // Convert the parsed date/time to UTC
        var utcDateTime = TimeZoneInfo.ConvertTimeToUtc(parsedDateTime, easternTimeZone);

        // Return the Unix timestamp (number of seconds since the Unix epoch, January 1st 1970 at 00:00:00 UTC)
        return (long)(utcDateTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
    }

    // Attempt to parse the input string as a date without a time
    if (DateTime.TryParseExact(date, formats, CultureInfo.InvariantCulture, DateTimeStyles.None,
            out DateTime parsedDate))
    {
        // Combine the parsed date with the current time to create a DateTime object with a time component
        DateTime currentDate = DateTime.Now;
        parsedDateTime = new DateTime(currentDate.Year, parsedDate.Month, parsedDate.Day, currentDate.Hour,
            currentDate.Minute, currentDate.Second);

        // Specify that the parsed date/time is in Eastern time
        var easternTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

        // Convert the parsed date/time to UTC
        var utcDateTime = TimeZoneInfo.ConvertTimeToUtc(parsedDateTime, easternTimeZone);

        // Return the Unix timestamp (number of seconds since the Unix epoch, January 1st 1970 at 00:00:00 UTC)
        return (long)(utcDateTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
    }

    // If the input string could not be parsed as a date, throw an exception
    throw new ArgumentException("Invalid date format", nameof(date));
}



    public DateTime ParseDate(string date)
    {
        DateTime parsedDate;
        if (!DateTime.TryParseExact(date, "MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate) &&
            !DateTime.TryParseExact(date, "MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
        {
            throw new ArgumentException("Invalid date format", nameof(date));
        }

        return parsedDate.ToUniversalTime();
    }


    public TimeSpan FormatTime(string time)
    {
        if (TimeSpan.TryParseExact(time, "hhmm", CultureInfo.InvariantCulture, out TimeSpan parsedTime))
        {
            return parsedTime;
        }

        // If the input string could not be parsed as a time, throw an exception
        throw new ArgumentException("Invalid time format", nameof(time));
    }
    
    public string ConvertToEasternTime(DateTime dateTime)
    {
        // Set the time zone to Eastern Time
        var easternTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

        // Convert the DateTime object to Eastern Time
        var convertedDateTime = TimeZoneInfo.ConvertTimeFromUtc(dateTime.ToUniversalTime(), easternTimeZone);

        // Format the converted DateTime as a string and return it
        return convertedDateTime.ToString("dddd, MMMM dd, yyyy h:mm tt");
    }
    
    public long DateTimeToUnixTime(DateTime dt)
    {
        var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return Convert.ToInt64((dt.ToUniversalTime() - epoch).TotalSeconds);
    }
}