using HomeSeerAPI;
using NullGuard;
using System.Collections.Generic;

namespace Hspi
{
    [NullGuard(ValidationFlags.Arguments | ValidationFlags.NonPublic)]
    internal class PrecipitationIntensityDeviceData : ScaledNumberDeviceData
    {
        public PrecipitationIntensityDeviceData(string name, XmlPathData pathData) :
            base(name, pathData)
        {
        }

        public override string GetDeviceSuffix(Unit unit)
        {
            return WUWeatherData.GetStringDescription(unit, DeviceUnitType.Precipitation);
        }

        public override IList<VSVGPairs.VGPair> GraphicsPairs => GetSingleGraphicsPairs("rainmeasure.png");
    }
}