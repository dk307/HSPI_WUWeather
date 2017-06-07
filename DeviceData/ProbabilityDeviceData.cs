using HomeSeerAPI;
using NullGuard;
using System.Collections.Generic;

namespace Hspi
{
    [NullGuard(ValidationFlags.Arguments | ValidationFlags.NonPublic)]
    internal abstract class ProbabilityDeviceData : NonScaledNumberDeviceData
    {
        public ProbabilityDeviceData(string name, XmlPathData pathData) :
            base(name, pathData)
        {
        }

        public override IList<VSVGPairs.VSPair> StatusPairs => GetNonScaledSingleStatusPairs(Suffix, 0, 100);

        protected override string Suffix => " %";
    }
}