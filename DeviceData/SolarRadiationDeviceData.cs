using HomeSeerAPI;
using NullGuard;
using System.Collections.Generic;

namespace Hspi
{
    [NullGuard(ValidationFlags.Arguments | ValidationFlags.NonPublic)]
    internal class SolarRadiationDeviceData : NonScaledNumberDeviceData
    {
        public SolarRadiationDeviceData(string name, XmlPathData pathData, string icon = "solarradiation.png") :
            base(name, pathData)
        {
            this.icon = icon;
        }

        protected override string Suffix => string.Empty;
        public override IList<VSVGPairs.VGPair> GraphicsPairs => GetSingleGraphicsPairs(icon);
        private readonly string icon;
    }
}