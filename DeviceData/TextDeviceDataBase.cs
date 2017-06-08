using HomeSeerAPI;
using NullGuard;
using Scheduler.Classes;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.XPath;

namespace Hspi
{
    using static System.FormattableString;

    [NullGuard(ValidationFlags.Arguments | ValidationFlags.NonPublic)]
    internal abstract class TextDeviceDataBase : DeviceData
    {
        public TextDeviceDataBase(string name, XmlPathData pathData) :
            base(name, pathData)
        {
        }

        protected void UpdateFirstNodeAsText(IHSApplication HS, DeviceClass device, [AllowNull] XPathNodeIterator value)
        {
            string stringValue = null;
            if ((value != null) && (value.MoveNext()))
            {
                stringValue = value.Current.ToString();
            }
            else
            {
                Trace.WriteLine(Invariant($"No node value found for {Name} Address [{device.get_Address(null)}]"));
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