using HomeSeerAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace Hspi
{
    /// <summary>
    /// Units of measurement supported by the Forecast service.
    /// </summary>
    public enum Unit
    {
        /// <summary>
        /// US units of measurement.
        /// </summary>
        [Description("US Measurements")]
        US,

        /// <summary>
        /// SI units of measurement.
        /// </summary>
        [Description("SI Measurements")]
        SI,
    }

    public class PluginConfig
    {
        public event EventHandler<EventArgs> ConfigChanged;

        public PluginConfig(IHSApplication HS)
        {
            this.HS = HS;

            apiKey = GetValue(APIKeyKey, string.Empty);
            refreshIntervalMinutes = GetValue(RefreshIntervalKey, 15);
            debugLogging = GetValue(DebugLoggingKey, false);
            stationId = GetValue<string>(StationIdKey, string.Empty);
            Enum.TryParse(GetValue(UnitIdKey, Unit.US.ToString()), out unit);

            enabledDevices = new Dictionary<string, IDictionary<string, bool>>();

            foreach (var deviceDefintion in WUWeatherData.DeviceDefinitions)
            {
                var childEnabled = new Dictionary<string, bool>();
                foreach (var childDeviceDefinition in deviceDefintion.Children)
                {
                    childEnabled[childDeviceDefinition.Name] = GetValue(childDeviceDefinition.Name, false, deviceDefintion.Name);
                }
                enabledDevices[deviceDefintion.Name] = childEnabled;
            }
        }

        public string APIKey
        {
            get
            {
                return apiKey;
            }

            set
            {
                SetValue(APIKeyKey, value, ref apiKey);
            }
        }

        public string StationId
        {
            get
            {
                return stationId;
            }

            set
            {
                SetValue(StationIdKey, value, ref stationId);
            }
        }

        public bool DebugLogging
        {
            get
            {
                return debugLogging;
            }

            set
            {
                SetValue(DebugLoggingKey, value, ref debugLogging);
            }
        }

        public int RefreshIntervalMinutes
        {
            get
            {
                return refreshIntervalMinutes;
            }

            set
            {
                SetValue(RefreshIntervalKey, value, ref refreshIntervalMinutes);
            }
        }

        public Unit Unit
        {
            get
            {
                return unit;
            }

            set
            {
                SetValue(UnitIdKey, value, ref unit);
            }
        }

        public bool GetEnabled(DeviceDataBase parent, DeviceDataBase child)
        {
            if (enabledDevices.TryGetValue(parent.Name, out var childEnabled))
            {
                if (childEnabled.TryGetValue(child.Name, out bool enabled))
                {
                    return enabled;
                }
            }

            return false;
        }

        public void SetEnabled(DeviceDataBase parent, DeviceDataBase child, bool enabled)
        {
            IDictionary<string, bool> childEnabled;
            if (enabledDevices.TryGetValue(parent.Name, out childEnabled))
            {
                if (childEnabled.ContainsKey(child.Name))
                {
                    bool tmpOldValue = childEnabled[child.Name];
                    SetValue(child.Name, enabled, ref tmpOldValue, parent.Name);
                    childEnabled[child.Name] = enabled;
                    return;
                }
            }

            throw new ArgumentException("Invalid values");
        }

        public string GetUnit(DeviceUnitType deviceUnit)
        {
            return WUWeatherData.GetStringDescription(this.Unit, deviceUnit);
        }

        private T GetValue<T>(string key, T defaultValue)
        {
            return GetValue(key, defaultValue, DefaultSection);
        }

        private T GetValue<T>(string key, T defaultValue, string section)
        {
            string stringValue = HS.GetINISetting(section, key, null, FileName);

            if (stringValue != null)
            {
                try
                {
                    T result = (T)System.Convert.ChangeType(stringValue, typeof(T), CultureInfo.InvariantCulture);
                    return result;
                }
                catch (Exception)
                {
                    return defaultValue;
                }
            }
            return defaultValue;
        }

        private void SetValue<T>(string key, T value, ref T oldValue)
        {
            SetValue<T>(key, value, ref oldValue, DefaultSection);
        }

        private void SetValue<T>(string key, T value, ref T oldValue, string section)
        {
            if (!value.Equals(oldValue))
            {
                string stringValue = System.Convert.ToString(value, CultureInfo.InvariantCulture);
                HS.SaveINISetting(section, key, stringValue, FileName);
                oldValue = value;
            }
        }

        public void FireConfigChanged()
        {
            if (ConfigChanged != null)
            {
                ConfigChanged(this, EventArgs.Empty);
            }
        }

        private const string APIKeyKey = "APIKey";
        private const string StationIdKey = "StationId";
        private const string UnitIdKey = "Unit";
        private const string DebugLoggingKey = "DebugLogging";
        private const string FileName = "WUWeather.ini";
        private const string RefreshIntervalKey = "RefreshIntervalMinutes";
        private const string DefaultSection = "Settings";

        private readonly IDictionary<string, IDictionary<string, bool>> enabledDevices;
        private readonly IHSApplication HS;
        private string apiKey;
        private bool debugLogging;
        private int refreshIntervalMinutes;
        private string stationId;
        private Unit unit;
    };
}