using HomeSeerAPI;
using Scheduler.Classes;
using System.Collections.Generic;
using System.ComponentModel;

namespace Hspi
{
    public class PrecipitationTypeDeviceData : DeviceData
    {
        public enum PrecipitationType
        {
            [Description("None")]
            None = -100,

            [Description("Rain")]
            Rain = -99,

            [Description("Snow")]
            Snow = -98,

            [Description("Sleet")]
            Sleet = -97,
        };

        public PrecipitationTypeDeviceData(string name, XmlPathData pathData) :
            base(name, pathData, initialValue: (double)PrecipitationType.None)
        {
        }

        public override void UpdateDeviceData(IHSApplication HS, DeviceClass device, System.Xml.XmlNodeList value)
        {
            //var data = jsonTokenValue == null ? null : jsonTokenValue.ToString();

            //int refId = device.get_Ref(HS);
            //if (!string.IsNullOrWhiteSpace(data))
            //{
            //    var value = FromString(data);
            //    HS.SetDeviceString(refId, null, false);
            //    HS.SetDeviceValueByRef(refId, (int)value, true);
            //}
            //else
            //{
            //    HS.SetDeviceValueByRef(refId, InitialValue, false);
            //    HS.SetDeviceString(refId, data, true);
            //}
        }

        public override IList<VSVGPairs.VSPair> GetStatusPairs(PluginConfig config) => GetStatusPairsForEnum<PrecipitationType>();

        public override IList<VSVGPairs.VGPair> GetGraphicsPairs(PluginConfig config) => GetGraphicsPairsForEnum<PrecipitationType>();

        private static PrecipitationType FromString(string data)
        {
            switch (data.ToLowerInvariant())
            {
                case "none": return PrecipitationType.None;
                case "rain": return PrecipitationType.Rain;
                case "snow": return PrecipitationType.Snow;
                case "sleet": return PrecipitationType.Sleet;
                default: return PrecipitationType.None;
            }
        }
    }
}