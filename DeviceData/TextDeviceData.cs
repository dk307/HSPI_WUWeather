using HomeSeerAPI;
using NullGuard;
using Scheduler.Classes;
using System.Collections.Generic;
using System.Xml.XPath;

namespace Hspi
{
    internal sealed class TextDeviceData : TextDeviceDataBase
    {
        public TextDeviceData(string name, XmlPathData pathData, string icon = "text.png")
            : base(name, pathData)
        {
            this.icon = icon;
        }

        public override void UpdateDeviceData(IHSApplication HS, DeviceClass device, [AllowNull]XPathNodeIterator value)
        {
            UpdateFirstNodeAsText(HS, device, value);
        }

        public override IList<VSVGPairs.VGPair> GraphicsPairs => GetSingleGraphicsPairs(icon);
        private readonly string icon;
    };
}