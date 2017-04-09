using HomeSeerAPI;
using Scheduler.Classes;
using System;

namespace Hspi
{
    public class DayDeviceData : TextDeviceData
    {
        public DayDeviceData(string name, XmlPathData pathData) :
           base(name, pathData)
        {
        }

        public override void UpdateDeviceData(IHSApplication HS, DeviceClass device, System.Xml.XmlNodeList value)
        {
            DateTimeOffset? dateTime = Parse(value);

            string deviceValue = dateTime.HasValue ? dateTime.Value.Date.ToLongDateString() : null;
            UpdateDeviceData(HS, device, deviceValue);
        }

        public static DateTimeOffset? Parse(System.Xml.XmlNodeList value)
        {
            DateTimeOffset? dateTime = null;
            if ((value != null) && (value.Count != 0))
            {
                string text = value.Item(0).InnerText;
                if (long.TryParse(text, out long epoch))
                {
                    dateTime = DateTimeOffset.FromUnixTimeSeconds(epoch).LocalDateTime;
                }
            }

            return dateTime;
        }
    }
}