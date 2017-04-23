using HomeSeerAPI;
using NullGuard;
using System.Collections.Generic;

namespace Hspi
{
    [NullGuard(ValidationFlags.Arguments | ValidationFlags.NonPublic)]
    internal class TemperatureDeviceData : ScaledNumberDeviceData
    {
        public TemperatureDeviceData(string name, XmlPathData pathData) :
            base(name, pathData)
        {
        }

        public override IList<VSVGPairs.VGPair> GraphicsPairs => GetSingleGraphicsPairs("temperature.png");

        public override string GetDeviceSuffix(Unit unit)
        {
            return WUWeatherData.GetStringDescription(unit, DeviceUnitType.Temperature);
        }
    }
}