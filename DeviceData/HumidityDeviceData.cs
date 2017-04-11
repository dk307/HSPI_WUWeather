using HomeSeerAPI;
using NullGuard;
using System.Collections.Generic;
using Scheduler.Classes;
using System.Xml;

namespace Hspi
{
    [NullGuard(ValidationFlags.Arguments | ValidationFlags.NonPublic)]
    internal class HumidityDeviceData : ProbabilityDeviceData
    {
        public HumidityDeviceData(string name, XmlPathData pathData) :
            base(name, pathData)
        {
        }

        public override void UpdateDeviceData(IHSApplication HS, DeviceClass device, [AllowNull] XmlNodeList value)
        {
            double? data = null;

            if ((value != null) && (value.Count != 0))
            {
                string text = value.Item(0).InnerText.Trim(new char[] { '%', ' ' });
                if (double.TryParse(text, out double doubleValue))
                {
                    data = doubleValue;
                }
            }

            UpdateDeviceData(HS, device, data);
        }

        public override IList<VSVGPairs.VGPair> GetGraphicsPairs(PluginConfig config) => GetSingleGraphicsPairs("humidity.png");
    }
}