using HomeSeerAPI;
using NullGuard;
using Scheduler.Classes;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Hspi
{
    [NullGuard(ValidationFlags.Arguments | ValidationFlags.NonPublic)]
    internal abstract class EnumBasedDeviceData<T> : DeviceData where T : struct, IConvertible
    {
        public EnumBasedDeviceData(string name, T defaultValue, XmlPathData pathData) :
            base(name, pathData)
        {
            this.defaultValue = defaultValue;
        }

        public override void SetInitialData(IHSApplication HS, DeviceClass device)
        {
            int refId = device.get_Ref(HS);
            HS.SetDeviceValueByRef(refId, defaultValue.ToDouble(CultureInfo.InvariantCulture), false);
        }

        public override void UpdateDeviceData(IHSApplication HS, DeviceClass device,
                                        [AllowNull]System.Xml.XPath.XPathNodeIterator value)
        {
            int refId = device.get_Ref(HS);
            bool valueSet = false;

            if ((value != null) && (value.MoveNext()))
            {
                var data = value == null ? null : value.Current.ToString();

                if (!string.IsNullOrWhiteSpace(data))
                {
                    HS.SetDeviceValueByRef(refId, FromString(data).ToDouble(CultureInfo.InvariantCulture), true);
                    valueSet = true;
                }
            }

            if (!valueSet)
            {
                HS.SetDeviceValueByRef(refId, defaultValue.ToDouble(CultureInfo.InvariantCulture), true);
            }
        }

        public override IList<VSVGPairs.VSPair> StatusPairs => GetStatusPairsForEnum(typeof(T));

        private T FromString(string data)
        {
            string upperData = data.ToUpperInvariant();
            foreach (var value in Enum.GetValues(typeof(T)))
            {
                if (value.ToString().ToUpperInvariant() == upperData)
                {
                    return (T)value;
                }
            }

            return defaultValue;
        }

        private readonly T defaultValue;
    }
}