using HomeSeerAPI;
using NullGuard;
using Scheduler.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Hspi
{
    [NullGuard(ValidationFlags.Arguments | ValidationFlags.NonPublic)]
    internal class WeatherTypeDeviceData : DeviceData
    {
        // https://www.wunderground.com/weather/api/d/docs?d=resources/phrase-glossary&MR=1
        public enum WeatherType
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

        public WeatherTypeDeviceData(string name, XmlPathData pathData) :
            base(name, pathData)
        {
        }

        public override void SetInitialData(IHSApplication HS, DeviceClass device)
        {
            int refId = device.get_Ref(HS);
            HS.SetDeviceValueByRef(refId, (int)WeatherType.Unknown, false);
        }

        public override void UpdateDeviceData(IHSApplication HS, DeviceClass device, [AllowNull]System.Xml.XPath.XPathNodeIterator value)
        {
            int refId = device.get_Ref(HS);
            bool valueSet = false;

            if ((value != null) && (value.MoveNext()))
            {
                var data = value == null ? null : value.Current.ToString();

                if (!string.IsNullOrWhiteSpace(data))
                {
                    HS.SetDeviceValueByRef(refId, (int)FromString(data), true);
                    valueSet = true;
                }
            }

            if (!valueSet)
            {
                HS.SetDeviceValueByRef(refId, (int)WeatherType.Unknown, true);
            }
        }

        public override IList<VSVGPairs.VSPair> StatusPairs => GetStatusPairsForEnum(typeof(WeatherType));

        public override IList<VSVGPairs.VGPair> GraphicsPairs => GetGraphicsPairsForEnum(typeof(WeatherType));

        private static WeatherType FromString(string data)
        {
            string upperData = data.ToUpperInvariant();
            foreach (var value in Enum.GetValues(typeof(WeatherType)))
            {
                if (value.ToString().ToUpperInvariant() == upperData)
                {
                    return (WeatherType)value;
                }
            }

            return WeatherType.Unknown;
        }
    }
}