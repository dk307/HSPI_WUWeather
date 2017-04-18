using HomeSeerAPI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;

namespace Hspi
{
    using static System.FormattableString;

    /// <summary>
    /// Class to store PlugIn Configuration
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    internal class PluginConfig : IDisposable
    {
        public event EventHandler<EventArgs> ConfigChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginConfig"/> class.
        /// </summary>
        /// <param name="HS">The homeseer application.</param>
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

        /// <summary>
        /// Gets or sets the API key for WU
        /// </summary>
        /// <value>
        /// The API key.
        /// </value>
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

        /// <summary>
        /// Gets or sets the station identifier.
        /// </summary>
        /// <value>
        /// The station identifier.
        /// </value>
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

        /// <summary>
        /// Gets or sets a value indicating whether debug logging is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [debug logging]; otherwise, <c>false</c>.
        /// </value>
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

        /// <summary>
        /// Gets or sets the refresh interval minutes.
        /// </summary>
        /// <value>
        /// The refresh interval minutes.
        /// </value>
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

        /// <summary>
        /// Gets or sets the unit of devices(SI or US).
        /// </summary>
        /// <value>
        /// The unit od devices.
        /// </value>
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

        /// <summary>
        /// Gets value indicating if device is enabled.
        /// </summary>
        /// <param name="parent">The root device.</param>
        /// <param name="child">The device.</param>
        /// <returns></returns>
        public bool GetDeviceEnabled(DeviceDataBase parent, DeviceDataBase child)
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

        /// <summary>
        /// Sets the device enabled value.
        /// </summary>
        /// <param name="parent">The parent device.</param>
        /// <param name="child">The device.</param>
        /// <param name="enabled">if set to <c>true</c> [enabled].</param>
        /// <exception cref="ArgumentException">Invalid values</exception>
        public void SetDeviceEnabled(DeviceDataBase parent, DeviceDataBase child, bool enabled)
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

        /// <summary>
        /// Gets the unit type description based on current Unit
        /// </summary>
        /// <param name="deviceUnit">The device unit.</param>
        /// <returns>Description of Unit</returns>
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

        /// <summary>
        /// Fires event that configuration changed.
        /// </summary>
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

        #endregion IDisposable Support

        private const string APIKeyKey = "APIKey";
        private const string StationIdKey = "StationId";
        private const string UnitIdKey = "Unit";
        private const string DebugLoggingKey = "DebugLogging";
        private readonly static string FileName = Invariant($"{Path.GetFileName(System.Reflection.Assembly.GetEntryAssembly().Location)}.ini");
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