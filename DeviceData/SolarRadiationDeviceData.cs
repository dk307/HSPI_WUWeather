using System;
using System.Collections.Generic;
using HomeSeerAPI;

namespace Hspi
{
    public class SolarRadiationDeviceData : NumberDeviceData
    {
        public SolarRadiationDeviceData(string name, XmlPathData pathData) :
            base(name, pathData)
        {
        }

        public override IList<VSVGPairs.VGPair> GetGraphicsPairs(PluginConfig config) => GetSingleGraphicsPairs("solarradiation.png");

        protected override string GetUnitString(PluginConfig config) => string.Empty;
    }
}