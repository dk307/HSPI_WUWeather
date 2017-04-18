using HomeSeerAPI;
using NullGuard;
using Scheduler.Classes;
using System;
using System.Collections.Generic;

namespace Hspi
{
    using static Hspi.StringUtil;

    /// <summary>
    ///  Base class for Root Devices
    /// </summary>
    /// <seealso cref="Hspi.DeviceDataBase" />
    [NullGuard(ValidationFlags.Arguments | ValidationFlags.NonPublic)]
    internal abstract class RootDeviceData : DeviceDataBase
    {
        protected RootDeviceData(string name, XmlPathData pathData) :
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

        public abstract void UpdateDeviceData(IHSApplication HS, DeviceClass device, System.Xml.XmlElement value);

        public abstract IReadOnlyCollection<DeviceData> Children { get; }
        public virtual DateTimeOffset? LastUpdateTime => null;

        public override IList<VSVGPairs.VGPair> GetGraphicsPairs(PluginConfig config) => GetSingleGraphicsPairs("root.png");

        public override string HSDeviceTypeString => INV($"{WUWeatherData.PlugInName} Root Device");
        public override string InitialString => "Root";
        public override double InitialValue => 0D;
        public override int HSDeviceType => (int)DeviceTypeInfo_m.DeviceTypeInfo.eDeviceType_Plugin.Root;
    };
}