using HomeSeerAPI;
using NullGuard;
using Scheduler.Classes;
using System.Collections.Generic;
using System.Xml.XPath;

namespace Hspi
{
    [NullGuard(ValidationFlags.Arguments | ValidationFlags.NonPublic)]
    internal abstract class TextDeviceDataBase : DeviceData
    {
        public TextDeviceDataBase(string name, XmlPathData pathData) :
            base(name, pathData)
        {
        }

        protected static void UpdateFirstNodeAsText(IHSApplication HS, DeviceClass device, [AllowNull] XPathNodeIterator value)
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
    }
}