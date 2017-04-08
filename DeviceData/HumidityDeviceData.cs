using System.Collections.Generic;
using HomeSeerAPI;
using Scheduler.Classes;

namespace Hspi
{
    public class HumidityDeviceData : ProbabilityDeviceData
    {
        public HumidityDeviceData(string name, XmlPathData pathData) :
            base(name, pathData)
        {
        }

        public override IList<VSVGPairs.VGPair> GetGraphicsPairs(PluginConfig config) => GetSingleGraphicsPairs("humidity.png");
    }
}