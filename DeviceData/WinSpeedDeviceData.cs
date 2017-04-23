using HomeSeerAPI;
using NullGuard;
using System.Collections.Generic;

namespace Hspi
{
    [NullGuard(ValidationFlags.Arguments | ValidationFlags.NonPublic)]
    internal class WindSpeedDeviceData : ScaledNumberDeviceData
    {
        public WindSpeedDeviceData(string name, XmlPathData pathData) :
            base(name, pathData)
        {
        }

        public override string GetDeviceSuffix(Unit unit)
        {
            return WUWeatherData.GetStringDescription(unit, DeviceUnitType.WinSpeed);
        }

        public override IList<VSVGPairs.VGPair> GetGraphicsPairs(PluginConfig config) => GetSingleGraphicsPairs("wind.png");
    }
}