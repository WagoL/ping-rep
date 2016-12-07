using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MesOpc.DAL.Models;

namespace MesOpc.DAL
{
    public class MesOpcContext : DbContext
    {
        public MesOpcContext() { } //empty con for ef

        public DbSet<Reply> Replies { get; set; }
    }
}