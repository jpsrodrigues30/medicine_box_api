using System.Collections.Generic;
using System;

namespace medicine_box_api.Domain.Dtos.Dictionary;
public static class WeekdayMapper
{
    private static readonly Dictionary<string, DayOfWeek> _map =
     new(StringComparer.OrdinalIgnoreCase)
     {
         ["Seg"] = DayOfWeek.Monday,
         ["Ter"] = DayOfWeek.Tuesday,
         ["Qua"] = DayOfWeek.Wednesday,
         ["Qui"] = DayOfWeek.Thursday,
         ["Sex"] = DayOfWeek.Friday,
         ["Sab"] = DayOfWeek.Saturday,
         ["Dom"] = DayOfWeek.Sunday,
     };

    /// <summary>
    /// Tenta converter a abreviação (ex: "Seg") para DayOfWeek.
    /// </summary>
    public static bool TryGetDay(string abbreviation, out DayOfWeek day)
        => _map.TryGetValue(abbreviation, out day);

    /// <summary>
    /// Retorna todos os dias válidos da aplicação.
    /// </summary>
    public static IReadOnlyDictionary<string, DayOfWeek> All => _map;
}
