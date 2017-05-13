using HomeSeerAPI;
using NullGuard;
using System.Collections.Generic;

namespace Hspi
{
    using static System.FormattableString;

    [NullGuard(ValidationFlags.Arguments | ValidationFlags.NonPublic)]
    internal abstract class ScaledNumberDeviceData : NumberDeviceData
    {
        public ScaledNumberDeviceData(string name, XmlPathData pathData) : base(name, pathData)
        {
        }

        public override IList<VSVGPairs.VSPair> StatusPairs
        {
            get
            {
                var pairs = new List<VSVGPairs.VSPair>();
                pairs.Add(new VSVGPairs.VSPair(HomeSeerAPI.ePairStatusControl.Status)
                {
                    PairType = VSVGPairs.VSVGPairType.Range,
                    RangeStart = int.MinValue,
                    RangeEnd = int.MaxValue,
                    IncludeValues = true,
                    RangeStatusDecimals = 2,
                    RangeStatusSuffix = Invariant($" {VSVGPairs.VSPair.ScaleReplace}"),
                    HasScale = true,
                });
                return pairs;
            }
        }

        public abstract string GetDeviceSuffix(Unit unit);
    }
}