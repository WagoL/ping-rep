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
        {
            eventLog1.WriteEntry("Starting pinging to remote endpoints", EventLogEntryType.SuccessAudit);
            _proces = new Thread(_monitor.Start);
            _proces.Start();
            eventLog1.WriteEntry("Started pinging to remote endpoints", EventLogEntryType.SuccessAudit);
        }

        protected override void OnStop()
        {
            eventLog1.WriteEntry("Stopping the monitoring application", EventLogEntryType.Information);
            _monitor.Stop();
            _proces.Join();
            eventLog1.WriteEntry("Stopped the monitoring application", EventLogEntryType.Information);
        }

    }
}
