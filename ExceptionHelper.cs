using System;
using System.Text;

namespace Hspi
{
    using Hspi.Exceptions;
    using static System.FormattableString;

    internal static class ExceptionHelper
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals")]
        public static string GetFullMessage(this Exception ex)
        {
            switch (ex)
            {
                case AggregateException aggregationException:
                    var stb = new StringBuilder();

                    foreach (var innerException in aggregationException.InnerExceptions)
                    {
                        stb.AppendLine(GetFullMessage(innerException));
                    }

                    return stb.ToString();

                case ApiKeyInvalidException apiKeyInvalidException:
                    return Invariant($"Invalid API Key. Check Configuration.");

                case StationIdInvalidException stationIdInvalidException:
                    return Invariant($"Station Is Not Valid or Offline. Check Configuration.");

                case WUWeatherDataInvalidException weatherException:
                    return Invariant($"Failed to get Weather Information with {weatherException.Message}");

                default:
                    return ex.Message;
            }
        }
    };
}