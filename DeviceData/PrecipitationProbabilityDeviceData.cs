using System.Collections.Generic;
using NullGuard;

using HomeSeerAPI;

namespace Hspi
{
    [NullGuard(ValidationFlags.Arguments | ValidationFlags.NonPublic)]
    public class PrecipitationProbabilityDeviceData : ProbabilityDeviceData
    {
        public PrecipitationProbabilityDeviceData(string name, XmlPathData pathData) :
            base(name, pathData)
        {
        }

        public override IList<VSVGPairs.VGPair> GetGraphicsPairs(PluginConfig config) => GetSingleGraphicsPairs("precipitationprobability.png");
    }
}