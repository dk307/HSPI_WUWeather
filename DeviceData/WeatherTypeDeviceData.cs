using HomeSeerAPI;
using NullGuard;
using System.Collections.Generic;
using System.ComponentModel;

namespace Hspi
{
    // https://www.wunderground.com/weather/api/d/docs?d=resources/phrase-glossary&MR=1
    internal enum WeatherType
    {
        [Description("Chance of Flurries")]
        ChanceFlurries = -200,

        [Description("Chance of Rain")]
        ChanceRain,

        [Description("Chance of Freezing Rain")]
        ChanceSleet,

        [Description("Chance of Snow")]
        ChanceSnow,

        [Description("Chance of Thunderstorms")]
        ChancetStorms,

        [Description("Clear")]
        Clear,

        [Description("Cloudy")]
        Cloudy,

        [Description("Flurries")]
        Flurries,

        [Description("Fog")]
        Fog,

        [Description("Haze")]
        Hazy,

        [Description("Mostly Cloudy")]
        MostlyCloudy,

        [Description("Mostly Sunny")]
        MostlySunny,

        [Description("Partly Cloudy")]
        PartlyCloudy,

        [Description("Partly Sunny")]
        PartlySunny,

        [Description("Freezing Rain")]
        Sleet,

        [Description("Rain")]
        Rain,

        [Description("Snow")]
        Snow,

        [Description("Sunny")]
        Sunny,

        [Description("Thunderstorms")]
        TStorms,

        [Description("Unknown")]
        Unknown,
    };

    [NullGuard(ValidationFlags.Arguments | ValidationFlags.NonPublic)]
    internal class WeatherTypeDeviceData : EnumBasedDeviceData<WeatherType>
    {
        public WeatherTypeDeviceData(string name, XmlPathData pathData) :
            base(name, WeatherType.Unknown, pathData)
        {
        }

        public override IList<VSVGPairs.VGPair> GraphicsPairs => GetGraphicsPairsForEnum(typeof(WeatherType));
    }
}