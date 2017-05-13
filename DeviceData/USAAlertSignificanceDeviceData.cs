using HomeSeerAPI;
using NullGuard;
using System.Collections.Generic;
using System.ComponentModel;

namespace Hspi
{
    internal enum USAAlertSignificanceType
    {
        [Description("Warning")]
        W = -100,

        [Description("Forecast")]
        F,

        [Description("Watch")]
        A,

        [Description("Outlook")]
        O,

        [Description("Advisory")]
        Y,

        [Description("Synopsis")]
        N,

        [Description("Statement")]
        S,

        [Description("None")]
        None = 0,
    };

    [NullGuard(ValidationFlags.Arguments | ValidationFlags.NonPublic)]
    internal class USAAlertSignificanceDeviceData : EnumBasedDeviceData<USAAlertSignificanceType>
    {
        public USAAlertSignificanceDeviceData(string name, XmlPathData pathData) :
            base(name, USAAlertSignificanceType.None, pathData)
        {
        }

        public override IList<VSVGPairs.VGPair> GraphicsPairs => GetSingleGraphicsPairs("weatherwarning.png");
    }
}