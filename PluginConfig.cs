using HomeSeerAPI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace Hspi
{
    internal class PluginConfig : IDisposable
    {
        public event EventHandler<EventArgs> ConfigChanged;

        public PluginConfig(IHSApplication HS)
        {
            this.HS = HS;

            apiKey = GetValue(APIKeyKey, string.Empty);
            refreshIntervalMinutes = GetValue(RefreshIntervalKey, 15);
            debugLogging = GetValue(DebugLoggingKey, false);
            stationId = GetValue<string>(StationIdKey, string.Empty);
            if (Enum.TryParse(GetValue(UnitIdKey, Unit.US.ToString()), out Unit readUnit))
            {
                unit = readUnit;
            }

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
                configLock.EnterReadLock();
                try
                {
                    return apiKey;
                }
                finally
                {
                    configLock.ExitReadLock();
                }
            }

            set
            {
                configLock.EnterWriteLock();
                try
                {
                    SetValue(APIKeyKey, value, ref apiKey);
                }
                finally
                {
                    configLock.ExitWriteLock();
                }
            }
        }

        public string StationId
        {
            get
            {
                configLock.EnterReadLock();
                try
                {
                    return stationId;
                }
                finally
                {
                    configLock.ExitReadLock();
                }
            }

            set
            {
                configLock.EnterWriteLock();
                try
                {
                    SetValue(StationIdKey, value, ref stationId);
                }
                finally
                {
                    configLock.ExitWriteLock();
                }
            }
        }

        public bool DebugLogging
        {
            get
            {
                configLock.EnterReadLock();
                try
                {
                    return debugLogging;
                }
                finally
                {
                    configLock.ExitReadLock();
                }
            }

            set
            {
                configLock.EnterWriteLock();
                try
                {
                    SetValue(DebugLoggingKey, value, ref debugLogging);
                }
                finally
                {
                    configLock.ExitWriteLock();
                }
            }
        }

        public int RefreshIntervalMinutes
        {
            get
            {
                configLock.EnterReadLock();
                try
                {
                    return refreshIntervalMinutes;
                }
                finally
                {
                    configLock.ExitReadLock();
                }
            }

            set
            {
                configLock.EnterWriteLock();
                try
                {
                    SetValue(RefreshIntervalKey, value, ref refreshIntervalMinutes);
                }
                finally
                {
                    configLock.ExitWriteLock();
                }
            }
        }

        public Unit Unit
        {
            get
            {
                configLock.EnterReadLock();
                try
                {
                    return unit;
                }
                finally
                {
                    configLock.ExitReadLock();
                }
            }

            set
            {
                configLock.EnterWriteLock();
                try
                {
                    SetValue(UnitIdKey, value, ref unit);
                }
                finally
                {
                    configLock.ExitWriteLock();
                }
            }
        }

        public bool GetEnabled(DeviceDataBase parent, DeviceDataBase child)
        {
            configLock.EnterReadLock();
            try
            {
                if (enabledDevices.TryGetValue(parent.Name, out var childEnabled))
                {
                    if (childEnabled.TryGetValue(child.Name, out bool enabled))
                    {
                        return enabled;
                    }
                }
            }
            finally
            {
                configLock.ExitReadLock();
            }

            return false;
        }

        public void SetEnabled(DeviceDataBase parent, DeviceDataBase child, bool enabled)
        {
            configLock.EnterWriteLock();
            try
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
            finally
            {
                configLock.ExitWriteLock();
            }
        }

        public string GetUnitDescription(DeviceUnitType deviceUnit)
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
                var ConfigChangedCopy = ConfigChanged;
                ConfigChangedCopy(this, EventArgs.Empty);
            }
        }

        #region IDisposable Support

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    configLock.Dispose();
                }
                disposedValue = true;
            }
        }


        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion


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
        private bool disposedValue = false;
        private readonly ReaderWriterLockSlim configLock = new ReaderWriterLockSlim();
    };
}