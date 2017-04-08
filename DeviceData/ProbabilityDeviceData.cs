using System.Collections.Generic;
using HomeSeerAPI;
using Scheduler.Classes;

namespace Hspi
{
    public class ProbabilityDeviceData : NumberDeviceData
    {
        public ProbabilityDeviceData(string name, XmlPathData pathData) :
            base(name, pathData)
        {
        }

        protected override string GetUnitString(PluginConfig config) => @" %";

        public override IList<VSVGPairs.VSPair> GetStatusPairs(PluginConfig config)
        {
            var pairs = new List<VSVGPairs.VSPair>();
            pairs.Add(new VSVGPairs.VSPair(HomeSeerAPI.ePairStatusControl.Status)
            {
                PairType = VSVGPairs.VSVGPairType.Range,
                RangeStart = 0,
                RangeEnd = 100,
                IncludeValues = true,
                RangeStatusDecimals = 0,
                RangeStatusSuffix = GetUnitString(config),
            });
            return pairs;
        }
    }
}