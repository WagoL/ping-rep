using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Threading;

namespace IpMonitor
{
    public partial class IpMonitorService : ServiceBase
    {
        private readonly Proces _monitor;
        private Thread _proces;
        ServiceStatus _serviceStatus = new ServiceStatus();
        public IpMonitorService()
        {
            InitializeComponent();
            eventLog1 = new EventLog();
            if (!EventLog.SourceExists("IpMonitor"))
                EventLog.CreateEventSource("IpMonitor","IP Monitoring");
            eventLog1.Source = "IpMonitor";
            eventLog1.Log = "IP Monitoring";
            eventLog1.WriteEntry("Loading up IP adresses.",EventLogEntryType.Information);
            try
            {
                _monitor = new Proces();
            }
            catch (IOException ioException)
            {
                eventLog1.WriteEntry("Error while loading IP's from txt file\n\n"+ioException,EventLogEntryType.Error);
            }
            eventLog1.WriteEntry("Done Loading up.",EventLogEntryType.SuccessAudit);
        }

        protected override void OnStart(string[] args)
        {            // Update the service state to Start Pending.  

            _serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            _serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref _serviceStatus);
            eventLog1.WriteEntry("Starting pinging to remote endpoints",EventLogEntryType.SuccessAudit);
            _proces = new Thread(_monitor.Start);
            // Update the service state to Running.  
            _serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref _serviceStatus);
        }

        protected override void OnStop()
        {
            _serviceStatus.dwCurrentState = ServiceState.SERVICE_STOP_PENDING;
            SetServiceStatus(this.ServiceHandle, ref _serviceStatus);
            _monitor.Stop();
            _proces.Join();
            eventLog1.WriteEntry("Stopping the monitoring application",EventLogEntryType.Information);
            _serviceStatus.dwCurrentState = ServiceState.SERVICE_STOPPED;
            SetServiceStatus(this.ServiceHandle, ref _serviceStatus);
        }

        public enum ServiceState
        {
            SERVICE_STOPPED = 0x00000001,
            SERVICE_START_PENDING = 0x00000002,
            SERVICE_STOP_PENDING = 0x00000003,
            SERVICE_RUNNING = 0x00000004,
            SERVICE_CONTINUE_PENDING = 0x00000005,
            SERVICE_PAUSE_PENDING = 0x00000006,
            SERVICE_PAUSED = 0x00000007,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ServiceStatus
        {
            public long dwServiceType;
            public ServiceState dwCurrentState;
            public long dwControlsAccepted;
            public long dwWin32ExitCode;
            public long dwServiceSpecificExitCode;
            public long dwCheckPoint;
            public long dwWaitHint;
        };
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);

    }
}
