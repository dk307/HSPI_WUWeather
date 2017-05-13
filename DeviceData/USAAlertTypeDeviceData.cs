using HomeSeerAPI;
using NullGuard;
using System.Collections.Generic;
using System.ComponentModel;

namespace Hspi
{
    internal enum USAAlertType
    {
        [Description("Hurricane Local Statement")]
        HUR,

        [Description("Tornado Warning")]
        TOR,

        [Description("Tornado Watch")]
        TOW,

        [Description("Severe Thunderstorm Warning")]
        WRN,

        [Description("Severe Thunderstorm Watch")]
        SEW,

        [Description("Winter Weather Advisory")]
        WIN,

        [Description("Flood Warning")]
        FLO,

        [Description("Flood Watch / Statement")]
        WAT,

        [Description("High Wind Advisory")]
        WND,

        [Description("Severe Weather Statement")]
        SVR,

        [Description("Heat Advisory")]
        HEA,

        [Description("Dense Fog Advisory")]
        FOG,

        [Description("Special Weather Statement")]
        SPE,

        [Description("Fire Weather Advisory")]
        FIR,

        [Description("Volcanic Activity Statement")]
        VOL,

        [Description("Hurricane Wind Warning")]
        HWW,

        [Description("Record Set")]
        REC,

        [Description("Public Reports")]
        REP,

        [Description("Public Information Statement")]
        PUB,

        [Description("None")]
        None = 0,
    };

    [NullGuard(ValidationFlags.Arguments | ValidationFlags.NonPublic)]
    internal class USAAlertTypeDeviceData : EnumBasedDeviceData<USAAlertType>
    {
        public USAAlertTypeDeviceData(string name, XmlPathData pathData) :
            base(name, USAAlertType.None, pathData)
        {
        }

        public override IList<VSVGPairs.VGPair> GraphicsPairs => GetSingleGraphicsPairs("weatherwarning.png");
    }
}