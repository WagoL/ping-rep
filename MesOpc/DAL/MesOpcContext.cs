using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;

namespace DAL
{
    public class MesOpcContext : DbContext
    {
        public MesOpcContext() : base(){} //empty con for ef

        public List<Reply> Replies { get; set; }
    }
}
