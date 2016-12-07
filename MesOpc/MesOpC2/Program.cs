using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DAL.Models;

namespace MesOpC2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (var db = new MesOpcContext())
            {
                db.Replies.Add(new Reply {Available = false, IpAddress = "10.0.33.32", Timestamp = DateTime.Now});
                db.SaveChanges();
            }
        }
    }
}
