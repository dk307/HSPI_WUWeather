using System.Collections.Generic;

namespace Hspi
{
    internal static class WUWeatherData
    {
        public const string PlugInName = @"WU Weather";
        public const string ImagesPathRoot = @"\images\wuweather\";

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
            switch (unit)
            {
                case Unit.US:
                    switch (deviceType)
                    {
                        case DeviceUnitType.Temperature:
                            return " °F";

                        case DeviceUnitType.Pressure:
                            return " Hg";

                        case DeviceUnitType.WinSpeed:
                            return " miles/hr";

                        case DeviceUnitType.Visibility:
                            return " miles";

                        case DeviceUnitType.Precipitation:
                            return " inches";

                        case DeviceUnitType.WeatherType:
                            break;
                    }
                    break;

                case Unit.SI:
                    switch (deviceType)
                    {
                        case DeviceUnitType.Temperature:
                            return " °C";

                        case DeviceUnitType.Pressure:
                            return " millibars";

                        case DeviceUnitType.WinSpeed:
                            return " km/hr";

                        case DeviceUnitType.Visibility:
                            return " km";

                        case DeviceUnitType.Precipitation:
                            return " mm";

                        case DeviceUnitType.WeatherType:
                            break;
                    }
                    break;
            }

            return string.Empty;
        }
    }
}