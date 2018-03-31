using System.Diagnostics.CodeAnalysis;

namespace NNG.Native.InteropTypes
{
    /// <summary>
    ///     nng_stat_type is used to determine the type of the statistic.
    ///     At present, only NNG_STAT_TYPE_LEVEL and and NNG_STAT_TYPE_COUNTER
    ///     are defined.  Counters generally increment, and therefore changes in the
    ///     value over time are likely more interesting than the actual level.  Level
    ///     values reflect some absolute state however, and should be presented to the
    ///     user as is.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Using native names of library")]
    [SuppressMessage("ReSharper", "EnumUnderlyingTypeIsInt", Justification = "Being explicit here.")]
    public enum nng_stat_type_enum : int
    {
        NNG_STAT_LEVEL = 0,

        NNG_STAT_COUNTER = 1
    }
}