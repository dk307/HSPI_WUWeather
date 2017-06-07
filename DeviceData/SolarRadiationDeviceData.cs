using HomeSeerAPI;
using NullGuard;
using Scheduler.Classes;
using System.Collections.Generic;
using System.Xml.XPath;

namespace Hspi
{
    [NullGuard(ValidationFlags.Arguments | ValidationFlags.NonPublic)]
    internal sealed class SolarRadiationDeviceData : NonScaledNumberDeviceData
    {
        public SolarRadiationDeviceData(string name, XmlPathData pathData, string icon = "solarradiation.png") :
            base(name, pathData)
        {
            this.icon = icon;
        }

        public override void UpdateDeviceData(IHSApplication HS, DeviceClass device, [AllowNull]XPathNodeIterator value)
        {
            UpdateFirstNodeAsNumber(HS, device, value);
        }

        protected override string Suffix => string.Empty;
        public override IList<VSVGPairs.VGPair> GraphicsPairs => GetSingleGraphicsPairs(icon);
        private readonly string icon;
    }
}