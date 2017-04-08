using HomeSeerAPI;
using System.Collections.Generic;

namespace Hspi
{
    public class WindBearingDeviceData : NumberDeviceData
    {
        public WindBearingDeviceData(string name, XmlPathData pathData) :
            base(name, pathData)
        {
        }

        protected override string GetUnitString(PluginConfig config) => string.Empty;

        public override IList<VSVGPairs.VGPair> GetGraphicsPairs(PluginConfig config) => GetSingleGraphicsPairs("windbearing.png");
    }
}