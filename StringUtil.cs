using NullGuard;
using System;

namespace Hspi
{
    /// <summary>
    ///  Helper function to Format string in Invariant way.
    /// </summary>
    [NullGuard(ValidationFlags.Arguments | ValidationFlags.NonPublic)]
    internal static class StringUtil
    {
        public static string INV(IFormattable formattable)
        {
            return formattable.ToString(null, System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}