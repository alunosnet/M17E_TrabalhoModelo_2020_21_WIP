using M17E_TrabalhoModelo_2020_21_WIP.Data;
using M17E_TrabalhoModelo_2020_21_WIP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace M17E_TrabalhoModelo_2020_21_WIP.Controllers
{
    [Authorize]
    public class ConsultasController : Controller
    {
        private M17E_TrabalhoModelo_2020_21_WIPContext db = new M17E_TrabalhoModelo_2020_21_WIPContext();

        // GET: Consultas
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Index")]
        public ActionResult PesquisaCliente()
        {
            string nome = Request.Form["nome"];
            var clientes = db.Clientes.Where(c => c.Nome.Contains(nome));

            return View("PesquisaCliente",clientes.ToList());
        }
        public ActionResult PesquisaDinamica()
        {
            return View();
        }
        public JsonResult PesquisaNome(string nome)
        {
            var clientes = db.Clientes.Where(c => c.Nome.Contains(nome)).ToList();
            var lista = new List<Campos>();
            foreach (var c in clientes)
                lista.Add(new Campos() { nome = c.Nome });
            
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public class Campos
        {
            public string nome { get; set; }
            public decimal valor { get; set; }
        }

        public ActionResult MelhorCliente()
        {
            using(var context=new M17E_TrabalhoModelo_2020_21_WIPContext())
            {
                string sql = @"SELECT nome,SUM(valor_pago) as valor
                                FROM Estadias INNER JOIN clientes
                                ON Estadias.clienteid=clientes.clienteid
                                GROUP BY estadias.clienteid,nome
                                ORDER BY valor DESC";
                var melhor = context.Database.SqlQuery<Campos>(sql);
                ViewBag.melhor = melhor.ToList()[0];
            }
            return View();
        }
        public ActionResult ClientesEFuncionarios()
        {
            var todos = db.Users.OrderBy(u => u.nome).Select(x => new Todos
            {
                id = x.UserID,
                nome = x.nome
            }).Union(db.Clientes.OrderBy(c => c.Nome).Select(x => new Todos
            {
                id = x.ClienteID,
                nome = x.Nome
            }));
            return View(todos.ToList());
        }
    }
}