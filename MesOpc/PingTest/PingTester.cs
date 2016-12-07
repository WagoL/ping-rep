using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace PingTest
{
    public class PingTester
    {
        public string IpAddress { get; }

        public PingTester(string ip)
        {
            IpAddress = ip;
        }

        public bool Execute()
        {
            bool pingAble = false;
            Ping pinger = new Ping();
            try
            {
                PingReply reply = pinger.Send(IpAddress);
                pingAble = reply.Status == IPStatus.Success;
            }
            catch (PingException) { }
            return pingAble;
        }

    }
}
