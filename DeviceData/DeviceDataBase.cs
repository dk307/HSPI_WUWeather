using HomeSeerAPI;
using Scheduler.Classes;
using System.Collections.Generic;
using System.IO;
using NullGuard;

namespace Hspi
{
    [NullGuard(ValidationFlags.Arguments | ValidationFlags.NonPublic)]
    public abstract class DeviceDataBase
    {
        protected DeviceDataBase(string name, XmlPathData pathData)
        {
            this.Name = name;
            this.PathData = pathData;
        }

        public abstract IList<VSVGPairs.VSPair> GetStatusPairs(PluginConfig config);

        public abstract IList<VSVGPairs.VGPair> GetGraphicsPairs(PluginConfig config);

        public abstract int HSDeviceType { get; }
        public abstract string HSDeviceTypeString { get; }
        public abstract string InitialString { get; }
        public abstract double InitialValue { get; }

        public string Name { get; private set; }
        public XmlPathData PathData { get; private set; }

        protected static IList<VSVGPairs.VGPair> GetSingleGraphicsPairs(string fileName)
        {
            var pairs = new List<VSVGPairs.VGPair>();
            pairs.Add(new VSVGPairs.VGPair()
            {
                PairType = VSVGPairs.VSVGPairType.Range,
                Graphic = Path.Combine(WUWeatherData.ImagesPathRoot, fileName),
                RangeStart = int.MinValue,
                RangeEnd = int.MaxValue,
            });
            return pairs;
        }

        protected void UpdateDeviceData(IHSApplication HS, DeviceClass device, double? data)
        {
            int refId = device.get_Ref(HS);

            if (data.HasValue)
            {
                HS.SetDeviceString(refId, null, false);
                HS.SetDeviceValueByRef(refId, data.Value, true);
            }
            else
            {
                HS.SetDeviceString(refId, InitialString, false);
                HS.SetDeviceValueByRef(refId, InitialValue, true);
            }
        }

        protected void UpdateDeviceData(IHSApplication HS, DeviceClass device, [AllowNull]string data)
        {
            int refId = device.get_Ref(HS);
            HS.SetDeviceValueByRef(refId, InitialValue, false);
            HS.SetDeviceString(refId, data, true);
        }
    };
}