using HomeSeerAPI;
using NullGuard;
using Scheduler.Classes;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Hspi
{
    [NullGuard(ValidationFlags.Arguments | ValidationFlags.NonPublic)]
    internal class EpochDeviceData : TextDeviceDataBase
    {
        public EpochDeviceData(string name, string displayFormat, XmlPathData pathData) :
           base(name, pathData)
        {
            this.displayFormat = displayFormat;
        }

        public override void UpdateDeviceData(IHSApplication HS, DeviceClass device, [AllowNull]System.Xml.XPath.XPathNodeIterator value)
        {
            DateTimeOffset? dateTime = Parse(value);

            double numberValue = 0;
            string stringValue = null;
            if (dateTime.HasValue)
            {
                numberValue = dateTime.Value.ToUnixTimeSeconds();
                stringValue = dateTime.Value.ToString(displayFormat, CultureInfo.CurrentCulture);
            }

            UpdateDeviceData(HS, device, numberValue);
            UpdateDeviceData(HS, device, stringValue);
        }

        public static DateTimeOffset? Parse([AllowNull]System.Xml.XPath.XPathNodeIterator value)
        {
            DateTimeOffset? dateTime = null;
            if ((value != null) && (value.MoveNext()))
            {
                string text = value.Current.ToString();
                if (long.TryParse(text, out long epoch))
                {
                    dateTime = DateTimeOffset.FromUnixTimeSeconds(epoch).LocalDateTime;
                }
            }

            return dateTime;
        }

        public override IList<VSVGPairs.VGPair> GraphicsPairs => GetSingleGraphicsPairs("clock.png");

        private readonly string displayFormat;
    }
}