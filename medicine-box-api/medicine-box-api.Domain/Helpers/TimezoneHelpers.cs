using System;

namespace medicine_box_api.Domain.Helpers;
public class TimezoneHelpers
{
    public static DateTime ToUtc(DateOnly day, TimeOnly time, TimeZoneInfo tz)
    {
        var localUnspecified = day.ToDateTime(time);                 
        var local = DateTime.SpecifyKind(localUnspecified, DateTimeKind.Unspecified);
        return TimeZoneInfo.ConvertTimeToUtc(local, tz);         
    }

    public static DateTime ClientNow(TimeZoneInfo tz)
    {
        return TimeZoneInfo.ConvertTime(DateTime.UtcNow, tz);      
    }

    public static DateTime ToClientLocal(DateTime utc, TimeZoneInfo tz)
    {
        if (utc.Kind != DateTimeKind.Utc) utc = DateTime.SpecifyKind(utc, DateTimeKind.Utc);
        return TimeZoneInfo.ConvertTimeFromUtc(utc, tz);
    }
}
