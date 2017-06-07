using HomeSeerAPI;
using NullGuard;
using Scheduler.Classes;
using System;
using System.Collections.Generic;
using System.Xml.XPath;

namespace Hspi
{
    [NullGuard(ValidationFlags.Arguments | ValidationFlags.NonPublic)]
    internal sealed class CalculatedHumidityDeviceData : ProbabilityDeviceData
    {
        public CalculatedHumidityDeviceData(string name, XmlPathData pathData, Func<double, double, double> calculator) :
            base(name, pathData)
        {
            this.calculator = calculator;
        }

        public override void UpdateDeviceData(IHSApplication HS, DeviceClass device, [AllowNull] XPathNodeIterator value)
        {
            UpdateByCalculatingAsNumber(HS, device, value, calculator);
        }

        public override IList<VSVGPairs.VGPair> GraphicsPairs => GetSingleGraphicsPairs("humidity.png");
        private Func<double, double, double> calculator;
    }
}