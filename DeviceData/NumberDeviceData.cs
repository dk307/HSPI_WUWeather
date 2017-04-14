using HomeSeerAPI;
using NullGuard;
using Scheduler.Classes;
using System.Collections.Generic;

namespace Hspi
{
    /// <summary>
    ///  Base class for Number Device
    /// </summary>
    /// <seealso cref="Hspi.DeviceData" />
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

        /// <summary>
        /// Gets the suffix for number device
        /// </summary>
        /// <param name="config">The plugin configuration.</param>
        /// <returns>Suffix used for the device</returns>
        protected abstract string GetUnitString(PluginConfig config);
    }
}