using System;
using TimeZoneConverter;

namespace medicine_box_api.Domain.Helpers;
public static class TimezoneResolver
{
    public static bool TryResolve(string? id, out TimeZoneInfo tz)
    {
        tz = TimeZoneInfo.Utc;
        if (string.IsNullOrWhiteSpace(id)) return false;

        try { tz = TZConvert.GetTimeZoneInfo(id.Trim()); return true; }
        catch
        {
            try { tz = TimeZoneInfo.FindSystemTimeZoneById(id.Trim()); return true; }
            catch { return false; }
        }
    }
}