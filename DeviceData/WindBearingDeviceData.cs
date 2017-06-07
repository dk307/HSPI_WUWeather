using HomeSeerAPI;
using NullGuard;
using Scheduler.Classes;
using System.Collections.Generic;
using System.Xml.XPath;

namespace Hspi
{
    [NullGuard(ValidationFlags.Arguments | ValidationFlags.NonPublic)]
    internal sealed class WindBearingDeviceData : NonScaledNumberDeviceData
    {
        public WindBearingDeviceData(string name, XmlPathData pathData) :
            base(name, pathData)
        {
        }

        protected override string Suffix => string.Empty;

        public override IList<VSVGPairs.VGPair> GraphicsPairs => GetSingleGraphicsPairs("windbearing.png");

        public override void UpdateDeviceData(IHSApplication HS, DeviceClass device, [AllowNull]XPathNodeIterator value)
        {
            UpdateFirstNodeAsNumber(HS, device, value);
        }
    }
}