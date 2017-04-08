using System.Collections.Generic;
using HomeSeerAPI;

namespace Hspi
{
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