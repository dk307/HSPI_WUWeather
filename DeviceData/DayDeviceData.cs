using HomeSeerAPI;
using NullGuard;
using Scheduler.Classes;
using System;

namespace Hspi
{
    [NullGuard(ValidationFlags.Arguments | ValidationFlags.NonPublic)]
    internal class DayDeviceData : TextDeviceData
    {
        public DayDeviceData(string name, XmlPathData pathData) :
           base(name, pathData)
        {
        }

        public override void UpdateDeviceData(IHSApplication HS, DeviceClass device, [AllowNull]System.Xml.XPath.XPathNodeIterator value)
        {
            DateTimeOffset? dateTime = Parse(value);

            string deviceValue = dateTime.HasValue ? dateTime.Value.Date.ToLongDateString() : null;
            UpdateDeviceData(HS, device, deviceValue);
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
    }
}