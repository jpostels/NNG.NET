using System.Diagnostics.CodeAnalysis;

namespace NNG.Native.InteropTypes
{
    /// <summary>
    ///     nng_stat_unit provides information about the unit for the statistic,
    ///     such as NNG_UNIT_BYTES or NNG_UNIT_BYTES.  If no specific unit is
    ///     applicable, such as a relative priority, then NN_UNIT_NONE is
    ///     returned.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Using native names of library")]
    [SuppressMessage("ReSharper", "EnumUnderlyingTypeIsInt", Justification = "Being explicit here.")]
    public enum nng_unit_enum : int
    {
        NNG_UNIT_NONE = 0,

        NNG_UNIT_BYTES = 1,

        NNG_UNIT_MESSAGES = 2,

        NNG_UNIT_BOOLEAN = 3,

        NNG_UNIT_MILLIS = 4,

        NNG_UNIT_EVENTS = 5
    }
}