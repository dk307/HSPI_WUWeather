using HomeSeerAPI;
using System.Collections.Generic;

namespace Hspi
{
    public class TemperatureDeviceData : NumberDeviceData
    {
        public TemperatureDeviceData(string name, XmlPathData pathData) :
            base(name, pathData)
        {
        }

        protected override string GetUnitString(PluginConfig config) => config.GetUnit(DeviceUnitType.Temperature);

        public override IList<VSVGPairs.VGPair> GetGraphicsPairs(PluginConfig config) => GetSingleGraphicsPairs("temperature.png");
    }
}