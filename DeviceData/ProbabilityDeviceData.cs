﻿using HomeSeerAPI;
using NullGuard;
using System.Collections.Generic;

namespace Hspi
{
    [NullGuard(ValidationFlags.Arguments | ValidationFlags.NonPublic)]
    internal class ProbabilityDeviceData : NonScaledNumberDeviceData
    {
        public ProbabilityDeviceData(string name, XmlPathData pathData) :
            base(name, pathData)
        {
        }

        public override IList<VSVGPairs.VSPair> GetStatusPairs(PluginConfig config) => GetNonScaledSingleStatusPairs(Suffix, 0, 100);

        protected override string Suffix => " %";
    }
}