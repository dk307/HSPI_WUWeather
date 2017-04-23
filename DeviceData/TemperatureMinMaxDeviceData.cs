using HomeSeerAPI;
using NullGuard;
using System.Collections.Generic;

namespace Hspi
{
    [NullGuard(ValidationFlags.Arguments | ValidationFlags.NonPublic)]
    internal class TemperatureMinMaxDeviceData : ScaledNumberDeviceData
    {
        public TemperatureMinMaxDeviceData(string name, XmlPathData pathData) :
            base(name, pathData)
        {
        }

        public override string GetDeviceSuffix(Unit unit)
        {
            return WUWeatherData.GetStringDescription(unit, DeviceUnitType.Temperature);
        }

        public override IList<VSVGPairs.VGPair> GraphicsPairs => GetSingleGraphicsPairs("thermometers.png");
    }
}