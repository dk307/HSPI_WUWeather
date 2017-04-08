using System.Collections.Generic;
using HomeSeerAPI;

namespace Hspi
{
    public class PressureTrendDeviceData : TextDeviceData
    {
        public PressureTrendDeviceData(string name, XmlPathData pathData) :
            base(name, pathData)
        {
        }

        public override IList<VSVGPairs.VGPair> GetGraphicsPairs(PluginConfig config) => GetSingleGraphicsPairs("pressure.png");
    }
}