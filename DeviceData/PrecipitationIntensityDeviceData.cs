using HomeSeerAPI;
using System.Collections.Generic;

namespace Hspi
{
    public class PrecipitationIntensityDeviceData : NumberDeviceData
    {
        public PrecipitationIntensityDeviceData(string name, XmlPathData pathData) :
            base(name, pathData)
        {
        }

        protected override string GetUnitString(PluginConfig config) => config.GetUnit(DeviceUnitType.Precipitation);

        public override IList<VSVGPairs.VGPair> GetGraphicsPairs(PluginConfig config) => GetSingleGraphicsPairs("rainmeasure.png");
    }
}