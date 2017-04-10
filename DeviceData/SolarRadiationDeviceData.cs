using HomeSeerAPI;
using NullGuard;
using System.Collections.Generic;

namespace Hspi
{
    [NullGuard(ValidationFlags.Arguments | ValidationFlags.NonPublic)]
    internal class SolarRadiationDeviceData : NumberDeviceData
    {
        public SolarRadiationDeviceData(string name, XmlPathData pathData) :
            base(name, pathData)
        {
        }

        public override IList<VSVGPairs.VGPair> GetGraphicsPairs(PluginConfig config) => GetSingleGraphicsPairs("solarradiation.png");

        protected override string GetUnitString(PluginConfig config) => string.Empty;
    }
}