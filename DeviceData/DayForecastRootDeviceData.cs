using HomeSeerAPI;
using NullGuard;
using Scheduler.Classes;
using System.Collections.Generic;

namespace Hspi
{
    [NullGuard(ValidationFlags.Arguments | ValidationFlags.NonPublic)]
    internal class DayForecastRootDeviceData : RootDeviceData
    {
        public DayForecastRootDeviceData(string name, XmlPathData pathData) :
            base(name, pathData)
        { }

        public override void UpdateDeviceData(IHSApplication HS, DeviceClass device, System.Xml.XmlElement value)
        {
            var childNavigator = value.CreateNavigator();
            var nodeIter = childNavigator.Select(dayDeviceData.PathData.GetPath(Unit.US));
            dayDeviceData.UpdateDeviceData(HS, device, nodeIter);
        }

        public override IReadOnlyCollection<DeviceData> Children => DayForecastWeatherDevices;

        private static readonly IReadOnlyCollection<DeviceData> DayForecastWeatherDevices = new List<DeviceData>()
        {
            new WeatherTypeDeviceData("Condition", new XmlPathData("icon")),
            new TemperatureMinMaxDeviceData("Max Temperature", new XmlPathData("high/fahrenheit", "high/celsius")),
            new TemperatureMinMaxDeviceData("Min Temperature", new XmlPathData("low/fahrenheit", "low/celsius")),
            new HumidityDeviceData("Average Humidity", new XmlPathData("avehumidity")),
            new PrecipitationProbabilityDeviceData("Precipitation Chance", new XmlPathData("pop")),
            new PrecipitationIntensityDeviceData("Precipitation Total", new XmlPathData("qpf_allday/in", "qpf_allday/mm")),
            new PrecipitationIntensityDeviceData("Precipitation Day", new XmlPathData("qpf_day/in", "qpf_day/mm")),
            new PrecipitationIntensityDeviceData("Precipitation Night", new XmlPathData("qpf_night/in", "qpf_night/mm")),
            new PrecipitationIntensityDeviceData("Snow Total", new XmlPathData("snow_allday/in", "snow_allday/mm")),
            new PrecipitationIntensityDeviceData("Snow Day", new XmlPathData("snow_day/in", "snow_day/mm")),
            new PrecipitationIntensityDeviceData("Snow Night", new XmlPathData("snow_night/in", "snow_night/mm")),
            new WindSpeedDeviceData("Wind Speed Average", new XmlPathData("avewind/mph", "avewind/kph")),
            new WindSpeedDeviceData("Wind Speed Maximum", new XmlPathData("maxwind/mph", "maxwind/kph")),
            new WindBearingDeviceData("Maximum Wind Direction", new XmlPathData("maxwind/degrees")),
        }.AsReadOnly();

        private readonly EpochDeviceData dayDeviceData = new EpochDeviceData("Date", "D", new XmlPathData("date/epoch"));
    }
}