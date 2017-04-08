using System.Collections.Generic;
using HomeSeerAPI;
using System.IO;

namespace Hspi
{
    public class VisiblityDeviceData : NumberDeviceData
    {
        public VisiblityDeviceData(string name, XmlPathData pathData) :
            base(name, pathData)
        {
        }

        protected override string GetUnitString(PluginConfig config) => config.GetUnit(DeviceUnitType.Visibility);

        public override IList<VSVGPairs.VGPair> GetGraphicsPairs(PluginConfig config) => GetSingleGraphicsPairs("visiblity.png");
    }
}