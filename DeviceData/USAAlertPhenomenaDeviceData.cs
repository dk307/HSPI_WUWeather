using HomeSeerAPI;
using NullGuard;
using System.Collections.Generic;
using System.ComponentModel;

namespace Hspi
{
    internal enum USAAlertPhenomena
    {
        [Description("Ashfall")]
        AF = -100,

        [Description("Air Stagnation")]
        AS,

        [Description("Blowing Snow")]
        BS,

        [Description("Brisk Wind")]
        BW,

        [Description("Blizzard")]
        BZ,

        [Description("Coastal Flood")]
        CF,

        [Description("Dust Storm")]
        DS,

        [Description("Blowing Dust")]
        DU,

        [Description("Extreme Cold")]
        EC,

        [Description("Excessive Heat")]
        EH,

        [Description("Extreme Wind")]
        EW,

        [Description("Areal Flood")]
        FA,

        [Description("Flash Flood")]
        FF,

        [Description("Dense Fog")]
        FG,

        [Description("Flood")]
        FL,

        [Description("Frost")]
        FR,

        [Description("Fire Weather")]
        FW,

        [Description("Freeze")]
        FZ,

        [Description("Gale")]
        GL,

        [Description("Hurricane Force Wind")]
        HF,

        [Description("Inland Hurricane")]
        HI,

        [Description("Heavy Snow")]
        HS,

        [Description("Heat")]
        HT,

        [Description("Hurricane")]
        HU,

        [Description("High Wind")]
        HW,

        [Description("Hydrologic")]
        HY,

        [Description("Hard Freeze")]
        HZ,

        [Description("Sleet")]
        IP,

        [Description("Ice Storm")]
        IS,

        [Description("Lake Effect Snow and Blowing Snow")]
        LB,

        [Description("Lake Effect Snow")]
        LE,

        [Description("Low Water")]
        LO,

        [Description("Lakeshore Flood")]
        LS,

        [Description("Lake Wind")]
        LW,

        [Description("Marine")]
        MA,

        [Description("Small Craft for Rough Bar")]
        RB,

        [Description("Snow and Blowing Snow")]
        SB,

        [Description("Small Craft")]
        SC,

        [Description("Hazardous Seas")]
        SE,

        [Description("Small Craft for Winds")]
        SI,

        [Description("Dense Smoke")]
        SM,

        [Description("Snow")]
        SN,

        [Description("Storm")]
        SR,

        [Description("High Surf")]
        SU,

        [Description("Severe Thunderstorm")]
        SV,

        [Description("Small Craft for Hazardous Seas")]
        SW,

        [Description("Inland Tropical Storm")]
        TI,

        [Description("Tornado")]
        TO,

        [Description("Tropical Storm")]
        TR,

        [Description("Tsunami")]
        TS,

        [Description("Typhoon")]
        TY,

        [Description("Ice Accretion")]
        UP,

        [Description("Wind Chill")]
        WC,

        [Description("Wind")]
        WI,

        [Description("Winter Storm")]
        WS,

        [Description("Winter Weather")]
        WW,

        [Description("Freezing Fog")]
        ZF,

        [Description("Freezing Rain")]
        ZR,

        [Description("None")]
        None = 0,
    };

    [NullGuard(ValidationFlags.Arguments | ValidationFlags.NonPublic)]
    internal class USAAlertPhenomenaDeviceData : EnumBasedDeviceData<USAAlertPhenomena>
    {
        public USAAlertPhenomenaDeviceData(string name, XmlPathData pathData) :
            base(name, USAAlertPhenomena.None, pathData)
        {
        }

        public override IList<VSVGPairs.VGPair> GraphicsPairs => GetSingleGraphicsPairs("weatherwarning.png");
    }
}