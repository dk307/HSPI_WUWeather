using HomeSeerAPI;
using HSCF.Communication.Scs.Communication;
using HSCF.Communication.Scs.Communication.EndPoints.Tcp;
using HSCF.Communication.ScsServices.Client;
using Hspi.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Hspi
{
    using static StringUtil;

    public abstract class HspiBase : IPlugInAPI2, IDisposable
    {
        protected HspiBase(string name, int capabilities = (int)Enums.eCapabilities.CA_IO, string instanceFriendlyName = "",
                           bool supportMutipleInstances = false, int accessLevel = 1, bool supportsMultipleInstancesSingleEXE = true,
                           bool supportsAddDevice = false, bool hsComPort = false, bool supportConfigDevice = false, bool supportConfigDeviceAll = false)
        {
            this.name = name;
            this.instanceFriendlyName = instanceFriendlyName;
            this.capabilities = capabilities;
            this.supportMutipleInstances = supportMutipleInstances;
            this.accessLevel = accessLevel;
            this.supportsMultipleInstancesSingleEXE = supportsMultipleInstancesSingleEXE;
            this.supportsAddDevice = supportsAddDevice;
            this.hsComPort = hsComPort;
            this.supportConfigDevice = supportConfigDevice;
            this.supportConfigDeviceAll = supportConfigDeviceAll;
        }

        public override bool HasTriggers => false;
        public override string Name => name;
        public override int TriggerCount => 0;
        public override bool Connected => HsClient.CommunicationState == CommunicationStates.Connected;

        protected IAppCallbackAPI Callback { get; private set; }
        protected IScsServiceClient<IAppCallbackAPI> CallbackClient { get; private set; }
        protected IScsServiceClient<IHSApplication> HsClient { get; private set; }
        protected IHSApplication HS { get; private set; }

        protected CancellationToken CancellationToken => cancellationTokenSource.Token;

        public override int AccessLevel() => accessLevel;

        public override string ActionBuildUI(string uniqueControlId, IPlugInAPI.strTrigActInfo actionInfo) => string.Empty;

        public override bool ActionConfigured(IPlugInAPI.strTrigActInfo actionInfo) => true;

        public override int ActionCount() => 0;

        public override string ActionFormatUI(IPlugInAPI.strTrigActInfo actionInfo) => string.Empty;

        public override IPlugInAPI.strMultiReturn ActionProcessPostUI(NameValueCollection postData,
                                                IPlugInAPI.strTrigActInfo actionInfo) => new IPlugInAPI.strMultiReturn();

        public override bool ActionReferencesDevice(IPlugInAPI.strTrigActInfo actionInfo, int deviceId) => false;

        public override int Capabilities() => capabilities;

        public override string ConfigDevice(int deviceId, string user, int userRights, bool newDevice) => string.Empty;

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "apiVersion",
        Justification = "This is what the old HS sample does.")]
        public void Connect(string serverAddress, int serverPort)
        {
            // This method is called by our console wrapper at launch time

            // Create our main connection to the homeseer TCP communication framework
            // part 1 - hs object Proxy
            try
            {
                HsClient = ScsServiceClientBuilder.CreateClient<IHSApplication>(new ScsTcpEndPoint(serverAddress, serverPort), this);
                HsClient.Connect();
                HS = HsClient.ServiceProxy;

                var apiVersion = HS.APIVersion; // just to make sure our connection is valid

                Task.Run(async () => await CheckConnection());
            }
            catch (Exception ex)
            {
                throw new HspiConnectionException(INV($"Error connecting homeseer SCS client: {ex.Message}"), ex);
            }

            // part 2 - callback object Proxy
            try
            {
                CallbackClient = ScsServiceClientBuilder.CreateClient<IAppCallbackAPI>(new ScsTcpEndPoint(serverAddress, serverPort), this);
                CallbackClient.Connect();
                Callback = CallbackClient.ServiceProxy;

                var apiVersion = Callback.APIVersion; // just to make sure our connection is valid
            }
            catch (Exception ex)
            {
                throw new HspiConnectionException(INV($"Error connecting callback SCS client: {ex.Message}"), ex);
            }

            // Establish the reverse connection from homeseer back to our plugin
            try
            {
                HS.Connect(Name, InstanceFriendlyName());
            }
            catch (Exception ex)
            {
                throw new HspiConnectionException(INV($"Error connecting homeseer to our plugin: {ex.Message}"), ex);
            }
        }

        private async Task CheckConnection()
        {
            while (!cancellationTokenSource.Token.IsCancellationRequested)
            {
                if (!Connected)
                {
                    cancellationTokenSource.Cancel();
                    break;
                }

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationTokenSource.Token).ConfigureAwait(false);
            }
        }

        public void WaitforShutDownOrDisconnect()
        {
            cancellationTokenSource.Token.WaitHandle.WaitOne();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public override string GenPage(string link) => string.Empty;

        public override string get_ActionName(int actionNumber) => string.Empty;

        public override bool get_Condition(IPlugInAPI.strTrigActInfo actionInfo) => false;

        public override bool get_HasConditions(int triggerNumber) => false;

        public override int get_SubTriggerCount(int triggerNumber) => 0;

        public override string get_SubTriggerName(int triggerNumber, int subTriggerNumber) => string.Empty;

        public override bool get_TriggerConfigured(IPlugInAPI.strTrigActInfo actionInfo) => true;

        public override string get_TriggerName(int triggerNumber) => string.Empty;

        public override string GetPagePlugin(string page, string user, int userRights, string queryString) => string.Empty;

        public override bool HandleAction(IPlugInAPI.strTrigActInfo actionInfo) => false;

        public override void HSEvent(Enums.HSEvent eventType, object[] parameters)
        {
        }

        public override string InstanceFriendlyName() => instanceFriendlyName;

        public override IPlugInAPI.strInterfaceStatus InterfaceStatus()
        {
            var s = new IPlugInAPI.strInterfaceStatus { intStatus = IPlugInAPI.enumInterfaceStatus.OK };
            return s;
        }

        public override string PagePut(string data) => string.Empty;

        public override object PluginFunction(string functionName, object[] parameters) => null;

        public override object PluginPropertyGet(string propertyName, object[] parameters) => null;

        public override void PluginPropertySet(string propertyName, object value)
        {
        }

        public override IPlugInAPI.PollResultInfo PollDevice(int deviceId)
        {
            var pollResult = new IPlugInAPI.PollResultInfo
            {
                Result = IPlugInAPI.enumPollResult.Device_Not_Found,
                Value = 0
            };

            return pollResult;
        }

        public override string PostBackProc(string page, string data, string user, int userRights) => string.Empty;

        public override bool RaisesGenericCallbacks() => false;

        public override SearchReturn[] Search(string searchString, bool regEx) => null;

        public override void set_Condition(IPlugInAPI.strTrigActInfo actionInfo, bool value)
        {
        }

        public override void SetDeviceValue(int deviceId, double value, bool trigger = true)
        {
            HS.SetDeviceValueByRef(deviceId, value, trigger);
        }

        public override void SetIOMulti(List<CAPI.CAPIControl> colSend)
        {
        }

        public override void ShutdownIO()
        {
            cancellationTokenSource.Cancel();

            this.HsClient.Disconnect();
            this.CallbackClient.Disconnect();
        }

        public override void SpeakIn(int deviceId, string text, bool wait, string host)
        {
        }

        public override bool SupportsAddDevice() => supportsAddDevice;

        public override bool SupportsConfigDevice() => supportConfigDevice;

        public override bool SupportsConfigDeviceAll() => supportConfigDeviceAll;

        public override bool SupportsMultipleInstances() => supportMutipleInstances;

        public override bool SupportsMultipleInstancesSingleEXE() => supportsMultipleInstancesSingleEXE;

        public override string TriggerBuildUI(string uniqueControlId, IPlugInAPI.strTrigActInfo triggerInfo) => string.Empty;

        public override string TriggerFormatUI(IPlugInAPI.strTrigActInfo actionInfo) => string.Empty;

        public override IPlugInAPI.strMultiReturn TriggerProcessPostUI(NameValueCollection postData,
            IPlugInAPI.strTrigActInfo actionInfo) => new IPlugInAPI.strMultiReturn();

        public override bool TriggerReferencesDevice(IPlugInAPI.strTrigActInfo actionInfo, int deviceId) => false;

        public override bool TriggerTrue(IPlugInAPI.strTrigActInfo actionInfo) => false;

        public override Enums.ConfigDevicePostReturn ConfigDevicePost(int deviceId,
            string data,
            string user,
            int userRights) => Enums.ConfigDevicePostReturn.DoneAndCancel;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    cancellationTokenSource.Dispose();
                    HsClient?.Dispose();
                    CallbackClient?.Dispose();
                }
                disposedValue = true;
            }
        }

        protected override bool GetHasTriggers() => false;

        protected override bool GetHscomPort() => hsComPort;

        protected override int GetTriggerCount() => 0;

        protected virtual void LogDebug(string message)
        {
            Trace.WriteLine(message);
            HS.WriteLog(Name, INV($"Debug:{message}"));
        }

        protected void LogError(string message)
        {
            Trace.TraceError(message);
            HS.WriteLog(Name, INV($"Error:{message}"));
        }

        protected void LogInfo(string message)
        {
            Trace.TraceInformation(message);
            HS.WriteLog(Name, message);
        }

        protected void LogWarning(string message)
        {
            Trace.TraceWarning(message);
            HS.WriteLog(Name, INV($"Warning:{message}"));
        }

        private readonly int accessLevel;
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly int capabilities;
        private readonly bool hsComPort;
        private readonly string instanceFriendlyName;
        private readonly string name;
        private readonly bool supportConfigDevice;
        private readonly bool supportConfigDeviceAll;
        private readonly bool supportMutipleInstances;
        private readonly bool supportsAddDevice;
        private readonly bool supportsMultipleInstancesSingleEXE;
        private bool disposedValue = false;
    }
}