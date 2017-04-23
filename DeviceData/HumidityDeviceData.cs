using HomeSeerAPI;
using NullGuard;
using Scheduler.Classes;
using System.Collections.Generic;
using System.Xml.XPath;

namespace Hspi
{
    [NullGuard(ValidationFlags.Arguments | ValidationFlags.NonPublic)]
    internal class HumidityDeviceData : ProbabilityDeviceData
    {
        public HumidityDeviceData(string name, XmlPathData pathData) :
            base(name, pathData)
        {
        }

        public override void UpdateDeviceData(IHSApplication HS, DeviceClass device, [AllowNull]XPathNodeIterator value)
        {
            double? data = null;

            if ((value != null) && (value.MoveNext()))
            {
                string text = value.Current.ToString().Trim(new char[] { '%', ' ' });
                if (double.TryParse(text, out double doubleValue))
                {
                    data = doubleValue;
                }
            }

            UpdateDeviceData(HS, device, data);
        }

        public override IList<VSVGPairs.VGPair> GraphicsPairs => GetSingleGraphicsPairs("humidity.png");
    }
}