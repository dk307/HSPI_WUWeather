using System.Collections.Generic;
using HomeSeerAPI;
using Scheduler.Classes;

namespace Hspi
{
    public abstract class NumberDeviceData : DeviceData
    {
        public NumberDeviceData(string name, XmlPathData pathData) :
            base(name, pathData)
        {
        }

        public override void UpdateDeviceData(IHSApplication HS, DeviceClass device, System.Xml.XmlNodeList value)
        {
            double? data = null;

            if ((value != null) && (value.Count != 0))
            {
                string text = value.Item(0).InnerText;
                double doubleValue;
                double.TryParse(text, out doubleValue);
                data = doubleValue;
            }

            UpdateDeviceData(HS, device, data);
        }

        public override IList<VSVGPairs.VSPair> GetStatusPairs(PluginConfig config)
        {
            var pairs = new List<VSVGPairs.VSPair>();
            pairs.Add(new VSVGPairs.VSPair(HomeSeerAPI.ePairStatusControl.Status)
            {
                PairType = VSVGPairs.VSVGPairType.Range,
                RangeStart = int.MinValue,
                RangeEnd = int.MaxValue,
                IncludeValues = true,
                RangeStatusDecimals = 2,
                RangeStatusSuffix = GetUnitString(config),
            });
            return pairs;
        }

        public override IList<VSVGPairs.VGPair> GetGraphicsPairs(PluginConfig config) => new List<VSVGPairs.VGPair>();

        protected abstract string GetUnitString(PluginConfig config);
    }
}