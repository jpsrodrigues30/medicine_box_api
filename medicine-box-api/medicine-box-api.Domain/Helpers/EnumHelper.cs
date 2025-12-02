using System;

namespace medicine_box_api.Domain.Helpers;
public class EnumHelper
{
    public static TEnum? TryParseEnum<TEnum>(string? input, bool ignoreCase = true)
        where TEnum : struct, Enum
    {
        if (string.IsNullOrWhiteSpace(input)) return null;

        return Enum.TryParse<TEnum>(input.Trim(), ignoreCase, out var value)
            ? value
            : null; // silent error => null
    }
}
