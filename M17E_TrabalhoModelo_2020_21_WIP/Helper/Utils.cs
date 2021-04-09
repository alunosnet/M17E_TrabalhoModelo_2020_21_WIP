using M17E_TrabalhoModelo_2020_21_WIP.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace M17E_TrabalhoModelo_2020_21_WIP.Helper
{
    public static class Utils
    {
        public static string UserId(this HtmlHelper htmlHelper,
            System.Security.Principal.IPrincipal utilizador)
        {
            string iduser = "";

            using (var context = new M17E_TrabalhoModelo_2020_21_WIPContext())
            {
                var consulta = context.Database.SqlQuery<int>(
                    "SELECT UserID FROM Users Where nome=@p0",
                    utilizador.Identity.Name);
                if (consulta.ToList().Count > 0)
                    iduser = string.Format("{0}", consulta.ToList()[0]);
            }

            return iduser;
        }
    }
}