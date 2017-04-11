using HomeSeerAPI;
using NullGuard;
using Scheduler.Classes;
using System.Collections.Generic;

namespace Hspi
{
    [NullGuard(ValidationFlags.Arguments | ValidationFlags.NonPublic)]
    internal abstract class NumberDeviceData : DeviceData
    {
        protected NumberDeviceData(string name, XmlPathData pathData) :
            base(name, pathData)
        {
        }

        public override void UpdateDeviceData(IHSApplication HS, DeviceClass device, [AllowNull] System.Xml.XmlNodeList value)
        {
            double? data = null;

            if ((value != null) && (value.Count != 0))
            {
                string text = value.Item(0).InnerText.Trim();
                if (double.TryParse(text, out double doubleValue))
                {
                    data = doubleValue;
                }
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