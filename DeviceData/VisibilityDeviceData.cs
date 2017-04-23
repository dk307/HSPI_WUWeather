using HomeSeerAPI;
using NullGuard;
using System.Collections.Generic;

namespace Hspi
{
    [NullGuard(ValidationFlags.Arguments | ValidationFlags.NonPublic)]
    internal class VisibilityDeviceData : ScaledNumberDeviceData
    {
        public VisibilityDeviceData(string name, XmlPathData pathData) :
            base(name, pathData)
        {
        }

        public override string GetDeviceSuffix(Unit unit)
        {
            return WUWeatherData.GetStringDescription(unit, DeviceUnitType.Visibility);
        }

        public override IList<VSVGPairs.VGPair> GetGraphicsPairs(PluginConfig config) => GetSingleGraphicsPairs("visiblity.png");
    }
}