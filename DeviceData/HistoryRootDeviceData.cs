using HomeSeerAPI;
using NullGuard;
using Scheduler.Classes;
using System.Collections.Generic;

namespace Hspi
{
    using static Hspi.StringUtil;

    [NullGuard(ValidationFlags.Arguments | ValidationFlags.NonPublic)]
    public class HistoryRootDeviceData : RootDeviceData
    {
        public HistoryRootDeviceData(string name, XmlPathData pathData) :
            base(name, pathData)
        { }

        public override void UpdateDeviceData(IHSApplication HS, DeviceClass device, [AllowNull]System.Xml.XmlNodeList value)
        {
        }

        public override IReadOnlyCollection<DeviceData> Children => HistoryWeatherDevices;

        private static string GetPathMax(string child)
        {
            return INV($"//observation/{child}[not(. <../preceding-sibling::observation/{child}) and not(. <../following-sibling::observation/{child})]");
        }

        private static string GetPathMin(string child)
        {
            return INV($"//observation/{child}[not(. >../preceding-sibling::observation/{child}) and not(. >../following-sibling::observation/{child})]");
        }

        private static readonly IReadOnlyCollection<DeviceData> HistoryWeatherDevices = new List<DeviceData>()
        {
            new TemperatureMinMaxDeviceData("Max Temperature", new XmlPathData(GetPathMax("tempi"),GetPathMax("tempm"))),
            new TemperatureMinMaxDeviceData("Min Temperature", new XmlPathData(GetPathMin("tempi"),GetPathMin("tempm"))),
            new HumidityDeviceData("Max Humidity", new XmlPathData(GetPathMax("hum"))),
            new HumidityDeviceData("Min Humidity", new XmlPathData(GetPathMin("hum"))),
            new PrecipitationIntensityDeviceData("Precipitation", new XmlPathData(GetPathMax("precip_totali"),GetPathMax("precip_totalm"))),
            new SolarRadiationDeviceData("Max Solar Radiation", new XmlPathData(GetPathMax("solarradiation"))),
         }.AsReadOnly();
    }
}