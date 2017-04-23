using HomeSeerAPI;
using NullGuard;
using Scheduler.Classes;
using System.Collections.Generic;
using System.Xml.XPath;

namespace Hspi
{
    [NullGuard(ValidationFlags.Arguments | ValidationFlags.NonPublic)]
    internal class TextDeviceData : DeviceData
    {
        public TextDeviceData(string name, XmlPathData pathData) :
            base(name, pathData)
        {
        }

        public override void UpdateDeviceData(IHSApplication HS, DeviceClass device, [AllowNull] XPathNodeIterator value)
        {
            string stringValue = null;
            if ((value != null) && (value.MoveNext()))
            {
                stringValue = value.Current.ToString();
            }
            UpdateDeviceData(HS, device, stringValue);
        }

        public override IList<VSVGPairs.VSPair> StatusPairs
        {
            get
            {
                var pairs = new List<VSVGPairs.VSPair>();
                pairs.Add(new VSVGPairs.VSPair(HomeSeerAPI.ePairStatusControl.Status)
                {
                    PairType = VSVGPairs.VSVGPairType.SingleValue,
                    Value = 0,
                    Status = "None",
                });
                return pairs;
            }
        }

        public override IList<VSVGPairs.VGPair> GraphicsPairs => GetSingleGraphicsPairs("text.png");
    }
}