using HomeSeerAPI;
using Scheduler.Classes;
using System.Collections.Generic;

namespace Hspi
{
    public class HistroyRootDeviceData : RootDeviceData
    {
        public HistroyRootDeviceData(string name, XmlPathData pathData) :
            base(name, pathData)
        { }

        public override void UpdateDeviceData(IHSApplication HS, DeviceClass device, System.Xml.XmlNodeList value)
        {
            if (value == null || value.Count == 0)
            {
                return;
            }
        }

        public override IReadOnlyCollection<DeviceData> Children => HistoryWeatherDevices;

        private static string GetPathMax(string child)
        {
            return $"//observation/{child}[not(. <../preceding-sibling::observation/{child}) and not(. <../following-sibling::observation/{child})]";
        }

        private static string GetPathMin(string child)
        {
            return $"//observation/{child}[not(. >../preceding-sibling::observation/{child}) and not(. >../following-sibling::observation/{child})]";
        }

        private static readonly IReadOnlyCollection<DeviceData> HistoryWeatherDevices = new List<DeviceData>()
        {
            new TemperatureMinMaxDeviceData("Max Temperature", new XmlPathData(GetPathMax("tempi"),GetPathMax("tempm"))),
            new TemperatureMinMaxDeviceData("Min Temperature", new XmlPathData(GetPathMin("tempi"),GetPathMin("tempm"))),
            new PrecipitationIntensityDeviceData("Precipitation", new XmlPathData(GetPathMax("precip_totali"),GetPathMax("precip_totalm"))),
            new HumidityDeviceData("Max Humidity", new XmlPathData(GetPathMax("hum"))),
            new HumidityDeviceData("Min Humidity", new XmlPathData(GetPathMin("hum"))),
            new SolarRadiationDeviceData("Max Solar Radiation", new XmlPathData(GetPathMax("solarradiation"))),
         }.AsReadOnly();

        private DayDeviceData dayDeviceData = new DayDeviceData("Date", new XmlPathData("date/epoch"));
    }
}