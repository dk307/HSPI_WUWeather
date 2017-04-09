using System.Collections.Generic;
using HomeSeerAPI;
using NullGuard;

namespace Hspi
{
    [NullGuard(ValidationFlags.Arguments | ValidationFlags.NonPublic)]
    public class TemperatureMinMaxDeviceData : NumberDeviceData
    {
        public TemperatureMinMaxDeviceData(string name, XmlPathData pathData) :
            base(name, pathData)
        {
        }

        protected override string GetUnitString(PluginConfig config) => config.GetUnit(DeviceUnitType.Temperature);

        public override IList<VSVGPairs.VGPair> GetGraphicsPairs(PluginConfig config) => GetSingleGraphicsPairs("thermometers.png");
    }
}