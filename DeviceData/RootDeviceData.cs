using System.Collections.Generic;
using HomeSeerAPI;
using System.IO;
using System;
using Scheduler.Classes;

namespace Hspi
{
    using static Hspi.StringUtil;

    public abstract class RootDeviceData : DeviceDataBase
    {
        public RootDeviceData(string name, XmlPathData pathData) :
            base(name, pathData)
        {
        }

        public override IList<VSVGPairs.VSPair> GetStatusPairs(PluginConfig config)
        {
            var pairs = new List<VSVGPairs.VSPair>();
            pairs.Add(new VSVGPairs.VSPair(HomeSeerAPI.ePairStatusControl.Status)
            {
                PairType = VSVGPairs.VSVGPairType.SingleValue,
                Value = 0,
            });
            return pairs;
        }

        public abstract void UpdateDeviceData(IHSApplication HS, DeviceClass device, System.Xml.XmlNodeList value);

        public abstract IReadOnlyCollection<DeviceData> Children { get; }
        public virtual DateTimeOffset? LastUpdateTime => null;

        public override IList<VSVGPairs.VGPair> GetGraphicsPairs(PluginConfig config) => GetSingleGraphicsPairs("root.png");

        public override string HSDeviceTypeString => INV($"{WUWeatherData.PlugInName} Root Device");
        public override string InitialString => "Root";
        public override double InitialValue => 0D;
        public override int HSDeviceType => (int)DeviceTypeInfo_m.DeviceTypeInfo.eDeviceType_Plugin.Root;
    };
}