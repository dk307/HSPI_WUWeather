using HomeSeerAPI;
using NullGuard;
using Scheduler.Classes;
using System.Collections.Generic;
using System.Xml.XPath;

namespace Hspi
{
    [NullGuard(ValidationFlags.Arguments | ValidationFlags.NonPublic)]
    internal sealed class WindSpeedDeviceData : ScaledNumberDeviceData
    {
        public WindSpeedDeviceData(string name, XmlPathData pathData) :
            base(name, pathData)
        {
        }

        public override string GetDeviceSuffix(Unit unit)
        {
            return WUWeatherData.GetStringDescription(unit, DeviceUnitType.WinSpeed);
        }

        public override IList<VSVGPairs.VGPair> GraphicsPairs => GetSingleGraphicsPairs("wind.png");

        public override void UpdateDeviceData(IHSApplication HS, DeviceClass device, [AllowNull]XPathNodeIterator value)
        {
            UpdateFirstNodeAsNumber(HS, device, value);
        }
    }
}