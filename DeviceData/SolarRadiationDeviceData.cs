using HomeSeerAPI;
using NullGuard;
using System.Collections.Generic;

namespace Hspi
{
    [NullGuard(ValidationFlags.Arguments | ValidationFlags.NonPublic)]
    internal class SolarRadiationDeviceData : NonScaledNumberDeviceData
    {
        public SolarRadiationDeviceData(string name, XmlPathData pathData) :
            base(name, pathData)
        {
        }

        protected override string Suffix => string.Empty;

        public override IList<VSVGPairs.VGPair> GetGraphicsPairs(PluginConfig config) => GetSingleGraphicsPairs("solarradiation.png");
    }
}