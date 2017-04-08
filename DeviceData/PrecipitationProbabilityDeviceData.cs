using System.Collections.Generic;
using HomeSeerAPI;

namespace Hspi
{
    public class PrecipitationProbabilityDeviceData : ProbabilityDeviceData
    {
        public PrecipitationProbabilityDeviceData(string name, XmlPathData pathData) :
            base(name, pathData)
        {
        }

        public override IList<VSVGPairs.VGPair> GetGraphicsPairs(PluginConfig config) => GetSingleGraphicsPairs("precipitationprobability.png");
    }
}