using HomeSeerAPI;
using NullGuard;
using Scheduler.Classes;
using System.Collections.Generic;
using System.IO;

namespace Hspi
{
    /// <summary>
    /// This is base class for creating and updating devices in HomeSeer.
    /// </summary>
    [NullGuard(ValidationFlags.Arguments | ValidationFlags.NonPublic)]
    internal abstract class DeviceDataBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceDataBase"/> class.
        /// </summary>
        /// <param name="name">Name of the Device</param>
        /// <param name="pathData">Path in xml used for selecting nodes in wu weather response.</param>
        protected DeviceDataBase(string name, XmlPathData pathData)
        {
            this.Name = name;
            this.PathData = pathData;
        }

        /// <summary>
        /// Gets the status pairs for creating device.
        /// </summary>
        ///
        /// <returns></returns>
        public abstract IList<VSVGPairs.VSPair> StatusPairs { get; }

        /// <summary>
        /// Gets the graphics pairs for creating device
        /// </summary>
        ///
        /// <returns></returns>
        public abstract IList<VSVGPairs.VGPair> GraphicsPairs { get; }

        public abstract int HSDeviceType { get; }
        public abstract string HSDeviceTypeString { get; }

        public string Name { get; private set; }
        public XmlPathData PathData { get; private set; }

        public abstract void SetInitialData(IHSApplication HS, DeviceClass device);

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

        /// <summary>
        /// Updates the device data from number data
        /// </summary>
        /// <param name="HS">Homeseer application.</param>
        /// <param name="device">The device to update.</param>
        /// <param name="data">Number data.</param>
        protected static void UpdateDeviceData(IHSApplication HS, DeviceClass device, double? data)
        {
            int refId = device.get_Ref(HS);

            if (data.HasValue && !inValidValues.Contains(data.Value))
            {
                HS.set_DeviceInvalidValue(refId, false);
                HS.SetDeviceValueByRef(refId, data.Value, true);
            }
            else
            {
                // do not update double value on no value.
                HS.set_DeviceInvalidValue(refId, true);
            }
        }

        /// <summary>
        /// Updates the device data from string data
        /// </summary>
        /// <param name="HS">Homeseer application.</param>
        /// <param name="device">The device to update.</param>
        /// <param name="data">string data.</param>
        protected static void UpdateDeviceData(IHSApplication HS, DeviceClass device, [AllowNull]string data)
        {
            int refId = device.get_Ref(HS);
            HS.set_DeviceInvalidValue(refId, false);
            HS.SetDeviceString(refId, data, true);
        }

        /// <summary>
        /// WU invalid values
        /// </summary>
        private static readonly SortedSet<double> inValidValues = new SortedSet<double> { -999.9D, -999D, -99, -99.99D,
                                                                                          -9999, -9999.99D, 99999,
                                                                                          -25375, -2539.7, };
    };
}