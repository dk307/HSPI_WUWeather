using System.Collections.Generic;
using System.Reflection;

namespace Hspi
{
    using static StringUtil;

    /// <summary>
    /// Class to store static data for WU
    /// </summary>
    internal static class WUWeatherData
    {
        /// <summary>
        /// The plugin name
        /// </summary>
        public const string PlugInName = @"WU Weather";

        /// <summary>
        /// The images path root for devices
        /// </summary>
        public const string ImagesPathRoot = @"\images\wuweather\";

        /// <summary>
        /// The device definitions for WU
        /// </summary>
        public static readonly IEnumerable<RootDeviceData> DeviceDefinitions = new List<RootDeviceData>()
        {
            new ConditionsRootDeviceData(),
            new DayForecastRootDeviceData("Day Forecast", new XmlPathData(@"response/forecast/simpleforecast/forecastdays/forecastday[period = 1]")),
            new DayForecastRootDeviceData("Tomorrow Forecast", new XmlPathData(@"response/forecast/simpleforecast/forecastdays/forecastday[period = 2]")),
            new DayForecastRootDeviceData("2nd Day Forecast", new XmlPathData(@"response/forecast/simpleforecast/forecastdays/forecastday[period = 3]")),
            new DayForecastRootDeviceData("3rd Day Forecast", new XmlPathData(@"response/forecast/simpleforecast/forecastdays/forecastday[period = 4]")),
            new HistoryRootDeviceData("Yesterday", new XmlPathData(@"response/history/observations")),
        };

        public static string GetStringDescription(Unit unit, DeviceUnitType deviceType)
        {
            FieldInfo fi = typeof(DeviceUnitType).GetField(deviceType.ToString());
            foreach (var attribute in (UnitTypeDescription[])fi.GetCustomAttributes(typeof(UnitTypeDescription), false))
            {
                if (attribute.Unit == unit)
                {
                    return INV($" {attribute.Description}");
                }
            }
            return string.Empty;
        }
    }
}