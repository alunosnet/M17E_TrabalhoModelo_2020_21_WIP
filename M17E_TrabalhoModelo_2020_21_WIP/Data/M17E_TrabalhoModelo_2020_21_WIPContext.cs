using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace M17E_TrabalhoModelo_2020_21_WIP.Data
{
    public class M17E_TrabalhoModelo_2020_21_WIPContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public M17E_TrabalhoModelo_2020_21_WIPContext() : base("name=M17E_TrabalhoModelo_2020_21_WIPContext")
        {
        }

        public System.Data.Entity.DbSet<M17E_TrabalhoModelo_2020_21_WIP.Models.Cliente> Clientes { get; set; }
    }
}
