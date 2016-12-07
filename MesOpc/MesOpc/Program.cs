﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MesOpc.DAL;
using MesOpc.DAL.Models;

namespace MesOpc
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new MesOpcContext())
            {
                db.Replies.Add(new Reply {Available = false,IpAddress = "10.0.32.34",Timestamp = DateTime.Now});
                db.SaveChanges();
            }
        }
    }
}
