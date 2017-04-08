using System;
using System.Collections.Generic;
using HomeSeerAPI;
using System.ComponentModel;
using System.IO;
using Scheduler.Classes;

namespace Hspi
{
    using static Hspi.StringUtil;

    public class WeatherTypeDeviceData : DeviceData
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
            base(name, pathData, initialValue: (double)WeatherType.Unknown)
        {
        }

        public override void UpdateDeviceData(IHSApplication HS, DeviceClass device, System.Xml.XmlNodeList value)
        {
            int refId = device.get_Ref(HS);
            bool valueSet = false;

            if ((value != null) && (value.Count != 0))
            {
                var data = value == null ? null : value.Item(0).InnerText;

                if (!string.IsNullOrWhiteSpace(data))
                {
                    HS.SetDeviceString(refId, null, false);
                    HS.SetDeviceValueByRef(refId, (int)FromString(data), true);
                    valueSet = true;
                }
            }

            if (!valueSet)
            {
                HS.SetDeviceValueByRef(refId, InitialValue, false);
                HS.SetDeviceString(refId, null, true);
            }
        }

        public override IList<VSVGPairs.VSPair> GetStatusPairs(PluginConfig config) => GetStatusPairsForEnum<WeatherType>();

        public override IList<VSVGPairs.VGPair> GetGraphicsPairs(PluginConfig config) => GetGraphicsPairsForEnum<WeatherType>();

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