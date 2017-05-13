using HomeSeerAPI;
using NullGuard;
using Scheduler.Classes;
using System;
using System.Collections.Generic;

namespace Hspi
{
    [NullGuard(ValidationFlags.Arguments | ValidationFlags.NonPublic)]
    internal class ConditionsRootDeviceData : RootDeviceData
    {
        public ConditionsRootDeviceData() :
            base("Current", new XmlPathData(@"response/current_observation"))
        { }

        public override void UpdateDeviceData(IHSApplication HS, DeviceClass device, System.Xml.XmlElement value)
        {
            var childNavigator = value.CreateNavigator();
            var nodeIter = childNavigator.Select(lastUpdatePath.GetPath(Unit.SI));

            lastUpdateTime = EpochDeviceData.Parse(nodeIter);
            if (lastUpdateTime.HasValue)
            {
                UpdateDeviceData(HS, device, lastUpdateTime.Value.ToString("G", System.Globalization.CultureInfo.CurrentCulture));
            }
        }

        public override IReadOnlyCollection<DeviceData> Children => CurrentWeatherDevices;
        public override DateTimeOffset? LastUpdateTime => lastUpdateTime;

        private static readonly IReadOnlyCollection<DeviceData> CurrentWeatherDevices = new List<DeviceData>()
        {
            new TextDeviceData("Summary", new XmlPathData("weather")),
            new WeatherTypeDeviceData("Condition", new XmlPathData("icon")),
            new TemperatureDeviceData("DewPoint", new XmlPathData("dewpoint_f", "dewpoint_c")),
            new TemperatureDeviceData("Temperature", new XmlPathData("temp_f", "temp_c")),
            new TemperatureDeviceData("Feel Like Temperature", new XmlPathData("feelslike_f", "feelslike_c")),
            new HumidityDeviceData("Humidity", new XmlPathData("relative_humidity")),
            new PrecipitationIntensityDeviceData("Precipitation 1hr", new XmlPathData("precip_1hr_in", "precip_1hr_metric")),
            new PrecipitationIntensityDeviceData("Precipitation Today", new XmlPathData("precip_today_in", "precip_today_metric")),
            new PressureDeviceData("Pressure", new XmlPathData("pressure_in", "pressure_mb")),
            new PressureTrendDeviceData("Pressure Trend", new XmlPathData("pressure_trend")),
            new SolarRadiationDeviceData("Solar Radiation", new XmlPathData("solarradiation")),
            new SolarRadiationDeviceData("UV", new XmlPathData("UV"), "uv.png"),
            new VisibilityDeviceData("Visiblity", new XmlPathData("visibility_mi", "visibility_km")),
            new TextDeviceData("Wind Summary", new XmlPathData("wind_string")),
            new WindBearingDeviceData("Wind Direction", new XmlPathData("wind_degrees")),
            new WindSpeedDeviceData("Wind Gust Speed", new XmlPathData("wind_gust_mph", "wind_gust_kph")),
            new WindSpeedDeviceData("Wind Speed", new XmlPathData("wind_mph", "wind_kph")),
         }.AsReadOnly();

        private readonly XmlPathData lastUpdatePath = new XmlPathData(@"observation_epoch");
        private DateTimeOffset? lastUpdateTime = null;
    }
}