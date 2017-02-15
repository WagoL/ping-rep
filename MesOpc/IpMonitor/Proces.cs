using DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DAL.Models;
using PingTest;

namespace IpMonitor
{
    public class Proces
    {
        private readonly List<PingTester> _pingTest = new List<PingTester>();
        private volatile bool _stop = false;
        private List<string> _ipaddresToMonitor;

        public string AllIps => string.Join("\n", _ipaddresToMonitor.ToArray());

        public Proces()
        {
            _ipaddresToMonitor = File.ReadAllLines(IpMonitor.Properties.Settings.Default.FilePathMonitorList).ToList();
            //init pingtester
            foreach (string ipAddress in _ipaddresToMonitor)
                _pingTest.Add(new PingTester(ipAddress));
        }
        public void Start()
        {
            //exec tests and save results to db
            using (var db = new MesOpcContext())
            {
                db.Database.Connection.ConnectionString = @"Data Source=sbelinfsqlp02\infp2012;Initial Catalog=Monitoring_INFRA;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                while (!_stop)
                {
                    foreach (PingTester tester in _pingTest)
                    {
                        bool testFailed = tester.Execute();
                        /*if (!testFailed)*/
                            db.Replies.Add(new Reply
                            {
                                Available = testFailed,
                                IpAddress = tester.IpAddress,
                                Timestamp = DateTime.Now
                            });
                    }
                    db.SaveChanges();
                    Thread.Sleep(250);
                }
            }
        }

        public void Stop()
        {
            _stop = true;
        }
    }
}
