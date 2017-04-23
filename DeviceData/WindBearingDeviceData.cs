using HomeSeerAPI;
using NullGuard;
using System.Collections.Generic;

namespace Hspi
{
    [NullGuard(ValidationFlags.Arguments | ValidationFlags.NonPublic)]
    internal class WindBearingDeviceData : NonScaledNumberDeviceData
    {
        public WindBearingDeviceData(string name, XmlPathData pathData) :
            base(name, pathData)
        {
        }

        protected override string Suffix => string.Empty;

        public override IList<VSVGPairs.VGPair> GraphicsPairs => GetSingleGraphicsPairs("windbearing.png");
    }
}