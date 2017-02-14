using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PingTest;
using DAL;
using DAL.Models;

namespace IpMonitor
{
    public partial class IpMonitorService : ServiceBase
    {
        private readonly Proces _monitor;
        private Thread _proces;
        public IpMonitorService()
        {
            _monitor = new Proces();
        }

        protected override void OnStart(string[] args)
        {
            _proces = new Thread(_monitor.Start);
        }

        protected override void OnStop()
        {
            _monitor.Stop();
            _proces.Join();
        }
    }
}
