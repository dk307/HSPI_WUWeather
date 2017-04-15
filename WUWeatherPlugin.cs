using HomeSeerAPI;
using Hspi.Exceptions;
using Hspi.WUWeather;
using NullGuard;
using Scheduler.Classes;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace Hspi
{
    using static Hspi.StringUtil;

    /// <summary>
    /// Plugin class for Weather Underground
    /// </summary>
    /// <seealso cref="Hspi.HspiBase" />
    [NullGuard(ValidationFlags.Arguments | ValidationFlags.NonPublic)]
    internal class WUWeatherPlugin : HspiBase
    {
        public WUWeatherPlugin()
            : base(WUWeatherData.PlugInName)
        {
        }

        public override string InitIO(string port)
        {
            string result = string.Empty;
            try
            {
                pluginConfig = new PluginConfig(HS);
                configPage = new ConfigPage(HS, pluginConfig);
                LogInfo("Starting Plugin");
#if DEBUG
                pluginConfig.DebugLogging = true;
#endif
                LogConfiguration();

                pluginConfig.ConfigChanged += PluginConfig_ConfigChanged;

                RegisterConfigPage();

                RestartPeriodicTask();

                LogDebug("Plugin Started");
            }
            catch (Exception ex)
            {
                result = INV($"Failed to initialize PlugIn With {ex.Message}");
                LogError(result);
            }

            return result;
        }

        private void LogConfiguration()
        {
            LogDebug(INV($"APIKey:{pluginConfig.APIKey} Refresh Interval:{pluginConfig.RefreshIntervalMinutes} Minutes Station:{pluginConfig.StationId}"));
        }

        private void PluginConfig_ConfigChanged(object sender, EventArgs e)
        {
            // Wait for 5 seconds before fetching the data to avoid sending too many requests
            // when user is doing a bunch of changes in config.
            RestartPeriodicTask(TimeSpan.FromSeconds(5));
        }

        protected override void LogDebug(string message)
        {
            if (pluginConfig.DebugLogging)
            {
                base.LogDebug(message);
            }
        }

        private static string CreateChildAddress(string parentAddress, string childAddress)
        {
            return INV($"{parentAddress}.{childAddress}");
        }

        /// <summary>
        /// Creates the HS device.
        /// </summary>
        /// <param name="parent">The data for parent of device.</param>
        /// <param name="rootDeviceData">The root device data.</param>
        /// <param name="deviceData">The device data.</param>
        /// <returns>New Device</returns>
        private DeviceClass CreateDevice([AllowNull]DeviceClass parent, [AllowNull]RootDeviceData rootDeviceData, DeviceDataBase deviceData)
        {
            if (rootDeviceData != null)
            {
                LogDebug(INV($"Creating {deviceData.Name} under {rootDeviceData.Name}"));
            }
            else
            {
                LogDebug(INV($"Creating Root {deviceData.Name}"));
            }

            DeviceClass device = null;
            int refId = HS.NewDeviceRef(deviceData.Name);
            if (refId > 0)
            {
                device = (DeviceClass)HS.GetDeviceByRef(refId);
                string address = rootDeviceData != null ? CreateChildAddress(rootDeviceData.Name, deviceData.Name) : deviceData.Name;
                device.set_Address(HS, address);
                device.set_Device_Type_String(HS, deviceData.HSDeviceTypeString);
                var deviceType = new DeviceTypeInfo_m.DeviceTypeInfo();
                deviceType.Device_API = DeviceTypeInfo_m.DeviceTypeInfo.eDeviceAPI.Plug_In;
                deviceType.Device_Type = deviceData.HSDeviceType;

                device.set_DeviceType_Set(HS, deviceType);
                device.set_Interface(HS, Name);
                device.set_InterfaceInstance(HS, string.Empty);
                device.set_Last_Change(HS, DateTime.Now);
                device.set_Location2(HS, parent != null ? parent.get_Name(HS) : deviceData.Name);
                device.set_Location(HS, Name);
                var pairs = deviceData.GetStatusPairs(pluginConfig);
                foreach (var pair in pairs)
                {
                    HS.DeviceVSP_AddPair(refId, pair);
                }

                var gPairs = deviceData.GetGraphicsPairs(pluginConfig);
                foreach (var gpair in gPairs)
                {
                    HS.DeviceVGP_AddPair(refId, gpair);
                }

                device.MISC_Set(HS, Enums.dvMISC.STATUS_ONLY);
                device.MISC_Set(HS, Enums.dvMISC.SHOW_VALUES);
                device.MISC_Clear(HS, Enums.dvMISC.AUTO_VOICE_COMMAND);
                device.MISC_Clear(HS, Enums.dvMISC.SET_DOES_NOT_CHANGE_LAST_CHANGE);
                device.set_Status_Support(HS, false);

                if (parent != null)
                {
                    parent.set_Relationship(HS, Enums.eRelationship.Parent_Root);
                    device.set_Relationship(HS, Enums.eRelationship.Child);
                    device.AssociatedDevice_Add(HS, parent.get_Ref(HS));
                    parent.AssociatedDevice_Add(HS, device.get_Ref(HS));
                }

                HS.SetDeviceValueByRef(refId, deviceData.InitialValue, false);
                HS.SetDeviceString(refId, deviceData.InitialString, false);
            }

            return device;
        }

        /// <summary>
        /// Creates the devices based on configuration.
        /// </summary>
        private void CreateDevices(CancellationToken token)
        {
            try
            {
                IDictionary<string, DeviceClass> currentDevices = GetCurrentDevices();
                foreach (var deviceDefinition in WUWeatherData.DeviceDefinitions)
                {
                    token.ThrowIfCancellationRequested();
                    currentDevices.TryGetValue(deviceDefinition.Name, out DeviceClass parentDevice);

                    foreach (var childDeviceDefinition in deviceDefinition.Children)
                    {
                        token.ThrowIfCancellationRequested();

                        if (!pluginConfig.GetDeviceEnabled(deviceDefinition, childDeviceDefinition))
                        {
                            continue;
                        }

                        // lazy creation of parent device when child is created
                        if (parentDevice == null)
                        {
                            parentDevice = CreateDevice(null, null, deviceDefinition);
                        }

                        string childAddress = CreateChildAddress(parentDevice.get_Address(HS), childDeviceDefinition.Name);

                        if (!currentDevices.ContainsKey(childAddress))
                        {
                            CreateDevice(parentDevice, deviceDefinition, childDeviceDefinition);
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                LogError(INV($"Failed to Create Devices For PlugIn With {ex.Message}"));
            }
        }

        /// <summary>
        /// Gets the current devices for plugin from Homeseer
        /// </summary>
        /// <returns>Current devices for plugin</returns>
        /// <exception cref="HspiException"></exception>
        private IDictionary<string, DeviceClass> GetCurrentDevices()
        {
            var deviceEnumerator = HS.GetDeviceEnumerator() as clsDeviceEnumeration;

            if (deviceEnumerator == null)
            {
                throw new HspiException(INV($"{Name} failed to get a device enumerator from HomeSeer."));
            }

            var currentDevices = new Dictionary<string, DeviceClass>();
            do
            {
                CancellationToken.ThrowIfCancellationRequested();
                DeviceClass device = deviceEnumerator.GetNext();
                if ((device != null) &&
                    (device.get_Interface(HS) != null) &&
                    (device.get_Interface(HS).Trim() == Name))
                {
                    string address = device.get_Address(HS);
                    currentDevices.Add(address, device);
                }
            } while (!deviceEnumerator.Finished);
            return currentDevices;
        }

        public override string GetPagePlugin(string page, [AllowNull]string user, int userRights, [AllowNull]string queryString)
        {
            if (page == ConfigPage.Name)
            {
                return configPage.GetWebPage();
            }

            return string.Empty;
        }

        public override string PostBackProc(string page, string data, [AllowNull]string user, int userRights)
        {
            if (page == ConfigPage.Name)
            {
                return configPage.PostBackProc(data, user, userRights);
            }

            return string.Empty;
        }

        #region "Script Override"

        public override object PluginFunction([AllowNull]string functionName, [AllowNull] object[] parameters)
        {
            switch (functionName)
            {
                case null:
                    return null;

                case "Refresh":
                    RestartPeriodicTask();
                    break;
            }
            return null;
        }

        #endregion "Script Override"

        #region "Action Override"

        public override int ActionCount()
        {
            return 1;
        }

        public override string get_ActionName(int actionNumber)
        {
            switch (actionNumber)
            {
                case ActionRefreshTANumber:
                    return INV($"{Name}:Refresh");

                default:
                    return base.get_ActionName(actionNumber);
            }
        }

        public override string ActionBuildUI([AllowNull]string uniqueControlId, IPlugInAPI.strTrigActInfo actionInfo)
        {
            switch (actionInfo.TANumber)
            {
                case ActionRefreshTANumber:
                    return string.Empty;

                default:
                    return base.ActionBuildUI(uniqueControlId, actionInfo);
            }
        }

        public override string ActionFormatUI(IPlugInAPI.strTrigActInfo actionInfo)
        {
            switch (actionInfo.TANumber)
            {
                case ActionRefreshTANumber:
                    return INV($"{WUWeatherData.PlugInName} Refreshes Data");

                default:
                    return base.ActionFormatUI(actionInfo);
            }
        }

        public override bool HandleAction(IPlugInAPI.strTrigActInfo actionInfo)
        {
            switch (actionInfo.TANumber)
            {
                case ActionRefreshTANumber:
                    RestartPeriodicTask();
                    return true;

                default:
                    return base.HandleAction(actionInfo);
            }
        }

        #endregion "Action Override"

        /// <summary>
        /// Restarts the periodic task to fetch data from server
        /// </summary>
        /// <param name="initialDelay">The initial one time delay.</param>
        private void RestartPeriodicTask(TimeSpan? initialDelay = null)
        {
            lock (periodicTaskLock)
            {
                if (periodicTask != null)
                {
                    cancellationTokenSourceForUpdateDevice.Cancel();
                    try
                    {
                        periodicTask.Wait(CancellationToken);
                    }
                    catch (AggregateException ex)
                    {
                        ex.Handle((exception) =>
                        {
                            if (exception is OperationCanceledException)
                            {
                                return true;
                            }
                            return false;
                        });
                    }
                    cancellationTokenSourceForUpdateDevice.Dispose();
                }

                cancellationTokenSourceForUpdateDevice = new CancellationTokenSource();
                periodicTask = CreateAndUpdateDevices(initialDelay); // dont wait
            }
        }

        /// <summary>
        /// Creates the and update devices.
        /// </summary>
        /// <param name="initialDelay">The initial one time delay.</param>
        /// <returns></returns>
        private async Task CreateAndUpdateDevices(TimeSpan? initialDelay)
        {
            using (var combinedToken = CancellationTokenSource.CreateLinkedTokenSource(CancellationToken, cancellationTokenSourceForUpdateDevice.Token))
            {
                while (!combinedToken.IsCancellationRequested)
                {
                    try
                    {
                        if (initialDelay.HasValue)
                        {
                            await Task.Delay(initialDelay.Value, combinedToken.Token);
                            initialDelay = null;
                        }
                        CreateDevices(combinedToken.Token);
                        await FetchAndUpdateDevices(combinedToken.Token).ConfigureAwait(false);
                    }
                    catch (OperationCanceledException)
                    {
                    }
                    catch (Exception ex)
                    {
                        LogWarning(INV($"Failed to Fetch Data with {ex.Message}"));
                    }

                    // Set it to run after RefreshIntervalMinutes minutes or next 12.00 am
                    TimeSpan nextRun = TimeSpan.FromMinutes(pluginConfig.RefreshIntervalMinutes);
                    TimeSpan nextDay = DateTimeOffset.Now.Date.AddDays(1) - DateTimeOffset.Now;

                    await Task.Delay(nextRun < nextDay ? nextRun : nextDay, combinedToken.Token).ConfigureAwait(false);
                }
            }
        }

        private async Task FetchAndUpdateDevices(CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(pluginConfig.APIKey) || string.IsNullOrWhiteSpace(pluginConfig.StationId))
            {
                LogWarning("Configuration not setup to fetch weather data");
                return;
            }

            LogDebug("Starting data fetch from WU Weather");
            LogConfiguration();

            WUWeatherService service = new WUWeatherService(pluginConfig.APIKey);
            var response = await service.GetDataForStationAsync(pluginConfig.StationId, token).ConfigureAwait(false);

            var existingDevices = GetCurrentDevices();
            foreach (var deviceDefinition in WUWeatherData.DeviceDefinitions)
            {
                CancellationToken.ThrowIfCancellationRequested();

                existingDevices.TryGetValue(deviceDefinition.Name, out var rootDevice);

                if (rootDevice == null)
                {
                    // no root device exists yet
                    continue;
                }

                var subObject = response.SelectNodes(deviceDefinition.PathData.GetPath(pluginConfig.Unit));

                if (subObject != null && subObject.Count != 0)
                {
                    deviceDefinition.UpdateDeviceData(HS, rootDevice, subObject);

                    DateTimeOffset? lastUpdate = deviceDefinition.LastUpdateTime;
                    foreach (var childDeviceDefinition in deviceDefinition.Children)
                    {
                        string childAddress = CreateChildAddress(deviceDefinition.Name, childDeviceDefinition.Name);
                        existingDevices.TryGetValue(childAddress, out var childDevice);

                        if (childDevice != null)
                        {
                            CancellationToken.ThrowIfCancellationRequested();
                            try
                            {
                                XmlNodeList elements = subObject.Item(0).SelectNodes(childDeviceDefinition.PathData.GetPath(pluginConfig.Unit));
                                childDeviceDefinition.UpdateDeviceData(HS, childDevice, elements);

                                if (lastUpdate.HasValue)
                                {
                                    childDevice.set_Last_Change(HS, lastUpdate.Value.DateTime);
                                }
                            }
                            catch (OperationCanceledException)
                            {
                            }
                            catch (Exception ex)
                            {
                                LogError($"Failed to update {childAddress} with {ex.Message}");
                            }
                        }
                    }
                }
            }
        }

        private void RegisterConfigPage()
        {
            string link = ConfigPage.Name;
            HS.RegisterPage(link, Name, string.Empty);

            HomeSeerAPI.WebPageDesc wpd = new HomeSeerAPI.WebPageDesc()
            {
                plugInName = Name,
                link = link,
                linktext = link,
                page_title = INV($"{Name} Config"),
            };
            Callback.RegisterConfigLink(wpd);
            Callback.RegisterLink(wpd);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (pluginConfig != null)
                {
                    pluginConfig.ConfigChanged -= PluginConfig_ConfigChanged;
                }
                cancellationTokenSourceForUpdateDevice.Dispose();
                if (configPage != null)
                {
                    configPage.Dispose();
                }

                if (pluginConfig != null)
                {
                    pluginConfig.Dispose();
                }
                periodicTask?.Dispose();

                disposedValue = true;
            }

            base.Dispose(disposing);
        }

        private CancellationTokenSource cancellationTokenSourceForUpdateDevice = new CancellationTokenSource();
        private Task periodicTask;
        private readonly object periodicTaskLock = new object();
        private ConfigPage configPage;
        private PluginConfig pluginConfig;
        private const int ActionRefreshTANumber = 1;
        private bool disposedValue = false;
    }
}