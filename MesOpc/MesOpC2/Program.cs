using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DAL;
using DAL.Models;
using PingTest;

namespace MesOpC2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            List<string> ipaddresToMonitor = new List<string>
            {
                "10.0.161.37",
                "10.0.161.47",
                "10.0.161.22",
                "10.0.161.48",
                "10.0.161.26",
                /*"10.0.161.43",*/ "10.0.161.56",
                "10.0.161.39",
                "10.0.161.45",
                "10.0.161.49",
                "10.0.34.25", //SBELMESOPCP01
                "10.0.200.66" //FortiGate-NF01
            };
            List<PingTester> pingTest = new List<PingTester>();
            //init pingtester
            foreach (string ipAddress in ipaddresToMonitor)
                pingTest.Add(new PingTester(ipAddress));

            //exec tests and save results to db
            using (var db = new MesOpcContext())
            {
                while (true)
                {
                    foreach (PingTester tester in pingTest)
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
    }
}
