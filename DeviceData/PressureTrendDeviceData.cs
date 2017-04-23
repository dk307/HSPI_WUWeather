using HomeSeerAPI;
using NullGuard;
using System.Collections.Generic;

namespace Hspi
{
    [NullGuard(ValidationFlags.Arguments | ValidationFlags.NonPublic)]
    internal class PressureTrendDeviceData : TextDeviceData
    {
        public PressureTrendDeviceData(string name, XmlPathData pathData) :
            base(name, pathData)
        {
        }

        public override IList<VSVGPairs.VGPair> GraphicsPairs => GetSingleGraphicsPairs("pressure.png");
    }
}