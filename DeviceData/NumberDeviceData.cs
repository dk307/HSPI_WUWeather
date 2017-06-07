using HomeSeerAPI;
using NullGuard;
using Scheduler.Classes;
using System;
using System.Collections.Generic;
using System.Xml.XPath;

namespace Hspi
{
    /// <summary>
    ///  Base class for Number Device
    /// </summary>
    /// <seealso cref="Hspi.DeviceData" />
    [NullGuard(ValidationFlags.Arguments | ValidationFlags.NonPublic)]
    internal abstract class NumberDeviceData : DeviceData
    {
        protected NumberDeviceData(string name, XmlPathData pathData) :
            base(name, pathData)
        {
        }

        protected static void UpdateFirstNodeAsNumber(IHSApplication HS, DeviceClass device, [AllowNull] XPathNodeIterator value)
        {
            double? data = null;

            if ((value != null) && (value.MoveNext()))
            {
                string text = value.Current.ToString().Trim(new char[] { '%', ' ' });
                if (double.TryParse(text, out double doubleValue))
                {
                    if (!inValidValues.Contains(doubleValue))
                    {
                        data = doubleValue;
                    }
                }
            }

            UpdateDeviceData(HS, device, data);
        }

        protected static void UpdateByCalculatingAsNumber(IHSApplication HS, DeviceClass device, [AllowNull] XPathNodeIterator value,
                                 Func<double, double, double> calculator)
        {
            double? data = null;

            while ((value != null) && (value.MoveNext()))
            {
                string text = value.Current.ToString().Trim(new char[] { '%', ' ' });
                if (double.TryParse(text, out double doubleValue))
                {
                    if (!inValidValues.Contains(doubleValue))
                    {
                        if (!data.HasValue)
                        {
                            data = doubleValue;
                        }
                        else
                        {
                            data = calculator(doubleValue, data.Value);
                        }
                    }
                }
            }

            UpdateDeviceData(HS, device, data);
        }

        /// <summary>
        /// WU invalid values
        /// </summary>
        private static readonly SortedSet<double> inValidValues = new SortedSet<double> { -999.9D, -999D, -99, -99.99D,
                                                                                          -9999, -9999.99D, 99999,
                                                                                          -25375, -2539.7, -573.3, 573.3 };
    }
}