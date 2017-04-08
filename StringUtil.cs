using System;

namespace Hspi
{
    internal static class StringUtil
    {
        public static string INV(IFormattable formattable)
        {
            return formattable.ToString(null, System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}