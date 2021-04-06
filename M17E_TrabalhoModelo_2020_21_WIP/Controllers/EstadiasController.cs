using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using M17E_TrabalhoModelo_2020_21_WIP.Data;
using M17E_TrabalhoModelo_2020_21_WIP.Models;

namespace M17E_TrabalhoModelo_2020_21_WIP.Controllers
{
    public class EstadiasController : Controller
    {
        private M17E_TrabalhoModelo_2020_21_WIPContext db = new M17E_TrabalhoModelo_2020_21_WIPContext();

        // GET: Estadias
        public async Task<ActionResult> Index()
        {
            var estadias = db.Estadias.Include(e => e.cliente).Include(e => e.quarto);
            return View(await estadias.ToListAsync());
        }

        // GET: Estadias/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estadia estadia = await db.Estadias.FindAsync(id);
            if (estadia == null)
            {
                return HttpNotFound();
            }
            //TODO: mostrar detalhes do cliente e do quarto
            return View(estadia);
        }

        // GET: Estadias/Create
        public ActionResult Create()
        {
            ViewBag.ClienteID = new SelectList(db.Clientes, "ClienteID", "Nome");
            //só incluir os quartos disponíveis
            ViewBag.QuartoID = new SelectList(db.Quartos.Where(q => q.estado==true), "ID", "ID");
            //TODO: criar manualmente os items

            return View();
        }

        // POST: Estadias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "EstadiaID,data_entrada,data_saida,valor_pago,ClienteID,QuartoID")] Estadia estadia)
        {
            if (ModelState.IsValid)
            {
                //data_saida=data_entrada
                estadia.data_saida = estadia.data_entrada;
                //valor_pago=0
                estadia.valor_pago = 0;
                db.Estadias.Add(estadia);
                //alterar o estado do quarto
                var quarto = db.Quartos.Find(estadia.QuartoID);
                quarto.estado = false;
                db.Entry(quarto).CurrentValues.SetValues(quarto);

                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ClienteID = new SelectList(db.Clientes, "ClienteID", "Nome", estadia.ClienteID);
            ViewBag.QuartoID = new SelectList(db.Quartos.Where(q => q.estado == true), "ID", "ID", estadia.QuartoID);
            return View(estadia);
        }

        // GET: Estadias/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Estadia estadia = await db.Estadias.FindAsync(id);
            if (estadia == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClienteID = new SelectList(db.Clientes, "ClienteID", "Nome", estadia.ClienteID);
            ViewBag.QuartoID = new SelectList(db.Quartos.Where(q => q.estado == true), "ID", "ID", estadia.QuartoID);
            return View(estadia);
        }

        // POST: Estadias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "EstadiaID,data_entrada,data_saida,valor_pago,ClienteID,QuartoID")] Estadia estadia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(estadia).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ClienteID = new SelectList(db.Clientes, "ClienteID", "Nome", estadia.ClienteID);
            ViewBag.QuartoID = new SelectList(db.Quartos.Where(q => q.estado == true), "ID", "ID", estadia.QuartoID);
            return View(estadia);
        }

        // GET: Estadias/Delete/5
        //public async Task<ActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Estadia estadia = await db.Estadias.FindAsync(id);
        //    if (estadia == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(estadia);
        //}

        //// POST: Estadias/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(int id)
        //{
        //    Estadia estadia = await db.Estadias.FindAsync(id);
        //    db.Estadias.Remove(estadia);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}

        //TODO:Lista das estadias em curso

        //TODO:processar saída

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
