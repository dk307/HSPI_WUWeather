using HomeSeerAPI;
using NullGuard;
using Scheduler.Classes;
using System.Collections.Generic;

namespace Hspi
{
    [NullGuard(ValidationFlags.Arguments | ValidationFlags.NonPublic)]
    internal class USAAlertsRootDeviceData : RootDeviceData
    {
        public USAAlertsRootDeviceData(string name, XmlPathData pathData) :
            base(name, pathData)
        { }

        public override void UpdateDeviceData(IHSApplication HS, DeviceClass device, System.Xml.XmlElement value)
        {
        }

        public override IReadOnlyCollection<DeviceData> Children => USAlertsChildrenDevices;

        private static readonly IReadOnlyCollection<DeviceData> USAlertsChildrenDevices = new List<DeviceData>()
        {
            new TextDeviceData("Alert 1 Message", new XmlPathData("alert[1]/message"), "weatherwarning.png"),
            new EpochDeviceData("Alert 1 Expires", "G", new XmlPathData("alert[1]/expires_epoch")),
            new USAAlertSignificanceDeviceData("Alert 1 Significance", new XmlPathData("alert[1]/significance")),
            new USAAlertPhenomenaDeviceData("Alert 1 Phenomena", new XmlPathData("alert[1]/phenomena")),
            new USAAlertTypeDeviceData("Alert 1 Type", new XmlPathData("alert[1]/type")),
            new TextDeviceData("Alert 1 Type Description", new XmlPathData("alert[1]/description"), "weatherwarning.png"),
            new TextDeviceData("Alert 2 Message", new XmlPathData("alert[2]/message"), "weatherwarning.png"),
            new EpochDeviceData("Alert 2 Expires", "G", new XmlPathData("alert[2]/expires_epoch")),
            new USAAlertSignificanceDeviceData("Alert 2 Significance", new XmlPathData("alert[2]/significance")),
            new USAAlertPhenomenaDeviceData("Alert 2 Phenomena", new XmlPathData("alert[2]/phenomena")),
            new USAAlertTypeDeviceData("Alert 2 Type", new XmlPathData("alert[2]/type")),
            new TextDeviceData("Alert 2 Type Description", new XmlPathData("alert[2]/description"), "weatherwarning.png"),
        }.AsReadOnly();
    }
}