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

        public Proces()
        {
            List<string> ipaddresToMonitor = File.ReadAllLines("IpToMonitor.txt").ToList();
            //init pingtester
            foreach (string ipAddress in ipaddresToMonitor)
                _pingTest.Add(new PingTester(ipAddress));
        }
        public void Start()
        {
            //exec tests and save results to db
            using (var db = new MesOpcContext())
            {
                while (!_stop)
                {
                    foreach (PingTester tester in _pingTest)
                    {
                        bool testFailed = tester.Execute();
                        if (!testFailed)
                            db.Replies.Add(new Reply
                            {
                                Available = false,
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
