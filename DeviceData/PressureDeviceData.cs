using System;
using System.Collections.Generic;
using HomeSeerAPI;
using Scheduler.Classes;
using System.ComponentModel;
using System.IO;

namespace Hspi
{
    public class PressureDeviceData : NumberDeviceData
    {
        public PressureDeviceData(string name, XmlPathData pathData) :
            base(name, pathData)
        {
        }

        protected override string GetUnitString(PluginConfig config) => config.GetUnit(DeviceUnitType.Pressure);

        public override IList<VSVGPairs.VGPair> GetGraphicsPairs(PluginConfig config) => GetSingleGraphicsPairs("pressure.png");
    }
}