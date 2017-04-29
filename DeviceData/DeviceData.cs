using HomeSeerAPI;
using NullGuard;
using Scheduler.Classes;
using System;
using System.Collections.Generic;
using System.IO;

namespace Hspi
{
    using static System.FormattableString;

    /// <summary>
    ///  Base class for Child Devices
    /// </summary>
    /// <seealso cref="Hspi.DeviceDataBase" />
    [NullGuard(ValidationFlags.Arguments | ValidationFlags.NonPublic)]
    internal abstract class DeviceData : DeviceDataBase
    {
        protected DeviceData(string name, XmlPathData pathData) :
            base(name, pathData)
        {
        }

        public override void SetInitialData(IHSApplication HS, DeviceClass device)
        {
            int refId = device.get_Ref(HS);
            HS.SetDeviceValueByRef(refId, 0D, false);
            HS.set_DeviceInvalidValue(refId, true);
        }

        /// <summary>
        /// Updates the device data from XML Node.
        /// </summary>
        /// <param name="HS">The HomeSeer application.</param>
        /// <param name="device">The device  to update.</param>
        /// <param name="value">XML Nodes Iterator containing values or null.</param>
        public abstract void UpdateDeviceData(IHSApplication HS, DeviceClass device, System.Xml.XPath.XPathNodeIterator value);

        public override int HSDeviceType => 0;
        public override string HSDeviceTypeString => Invariant($"{WUWeatherData.PlugInName} Information Device");

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification = "Need for Filename")]
        protected static IList<VSVGPairs.VGPair> GetGraphicsPairsForEnum(Type enumType)
        {
            var pairs = new List<VSVGPairs.VGPair>();
            foreach (var value in Enum.GetValues(enumType))
            {
                var pair = new VSVGPairs.VGPair()
                {
                    PairType = VSVGPairs.VSVGPairType.SingleValue,
                    Graphic = Path.Combine(WUWeatherData.ImagesPathRoot, Invariant($"{value.ToString().ToLowerInvariant()}.png")),
                    Set_Value = (int)value,
                };
                pairs.Add(pair);
            }
            return pairs;
        }

        protected static IList<VSVGPairs.VSPair> GetStatusPairsForEnum(Type enumType)
        {
            var pairs = new List<VSVGPairs.VSPair>();
            foreach (var value in Enum.GetValues(enumType))
            {
                pairs.Add(new VSVGPairs.VSPair(HomeSeerAPI.ePairStatusControl.Status)
                {
                    PairType = VSVGPairs.VSVGPairType.SingleValue,
                    Value = (int)value,
                    Status = EnumHelper.GetDescription((System.Enum)value)
                });
            }

            return pairs;
        }
    };
}