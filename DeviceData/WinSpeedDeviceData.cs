using HomeSeerAPI;
using System.Collections.Generic;

namespace Hspi
{
    public class WindSpeedDeviceData : NumberDeviceData
    {
        public WindSpeedDeviceData(string name, XmlPathData pathData) :
            base(name, pathData)
        {
        }

        protected override string GetUnitString(PluginConfig config) => config.GetUnit(DeviceUnitType.WinSpeed);

        public override IList<VSVGPairs.VGPair> GetGraphicsPairs(PluginConfig config) => GetSingleGraphicsPairs("wind.png");
    }
}