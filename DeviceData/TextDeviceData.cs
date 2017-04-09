using System.Collections.Generic;
using HomeSeerAPI;
using Scheduler.Classes;
using NullGuard;

namespace Hspi
{
    [NullGuard(ValidationFlags.Arguments | ValidationFlags.NonPublic)]
    public class TextDeviceData : DeviceData
    {
        public TextDeviceData(string name, XmlPathData pathData) :
            base(name, pathData)
        {
        }

        public override void UpdateDeviceData(IHSApplication HS, DeviceClass device, [AllowNull] System.Xml.XmlNodeList value)
        {
            string stringValue = null;
            if ((value != null) && (value.Count != 0))
            {
                stringValue = value.Item(0).InnerText;
            }
            UpdateDeviceData(HS, device, stringValue);
        }

        public override IList<VSVGPairs.VSPair> GetStatusPairs(PluginConfig config)
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

        public override IList<VSVGPairs.VGPair> GetGraphicsPairs(PluginConfig config) => GetSingleGraphicsPairs("text.png");
    }
}