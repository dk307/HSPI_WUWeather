using HomeSeerAPI;
using NullGuard;
using Scheduler.Classes;
using System.Collections.Generic;

namespace Hspi
{
    using System;
    using static System.FormattableString;

    [NullGuard(ValidationFlags.Arguments | ValidationFlags.NonPublic)]
    internal class HistoryRootDeviceData : RootDeviceData
    {
        public HistoryRootDeviceData(string name, XmlPathData pathData) :
            base(name, pathData)
        { }

        public override IReadOnlyCollection<DeviceData> Children => HistoryWeatherDevices;

        public override void UpdateDeviceData(IHSApplication HS, DeviceClass device, System.Xml.XmlElement value)
        {
        }

        private static string GetPathMax(string child)
        {
            return Invariant($"//observation/{child}[not(. <../preceding-sibling::observation/{child}) and not(. <../following-sibling::observation/{child})]");
        }

        private static string GetPathMin(string child)
        {
            return Invariant($"//observation/{child}[not(. >../preceding-sibling::observation/{child}) and not(. >../following-sibling::observation/{child})]");
        }

        private static readonly IReadOnlyCollection<DeviceData> HistoryWeatherDevices = new List<DeviceData>()
        {
            new CalculatedTemperatureDeviceData("Max Temperature", new XmlPathData("//observation/tempi" ,"//observation/tempm"), Math.Max),
            new CalculatedTemperatureDeviceData("Min Temperature", new XmlPathData("//observation/tempi" ,"//observation/tempm"), Math.Min),
            new CalculatedHumidityDeviceData("Max Humidity", new XmlPathData("//observation/hum"), Math.Max),
            new CalculatedHumidityDeviceData("Min Humidity", new XmlPathData("//observation/hum"), Math.Min),
            new CalculatedPrecipitationIntensityDeviceData("Precipitation", new XmlPathData("//observation/precip_totali", "//observation/precip_totalm"), Math.Max),
            new CalculatedSolarRadiationDeviceData("Max Solar Radiation", new XmlPathData("//observation/solarradiation"), Math.Max),
         }.AsReadOnly();
    }
}