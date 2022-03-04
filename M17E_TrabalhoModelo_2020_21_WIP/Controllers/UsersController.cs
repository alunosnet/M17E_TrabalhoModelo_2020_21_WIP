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
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;

namespace M17E_TrabalhoModelo_2020_21_WIP.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private M17E_TrabalhoModelo_2020_21_WIPContext db = new M17E_TrabalhoModelo_2020_21_WIPContext();

        // GET: Users
        [Authorize(Roles ="Administrador")]
        public async Task<ActionResult> Index()
        {
            return View(await db.Users.ToListAsync());
        }

        // GET: Users/Details/5
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        [Authorize(Roles = "Administrador")]
        public ActionResult Create()
        {
            //perfil
            var utilizador = new User();
            utilizador.perfis = new[]
            {
                new SelectListItem{Value="0",Text="Administrador" },
                new SelectListItem{Value="1",Text="Utilizador"}
            };
            return View(utilizador);
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Create([Bind(Include = "UserID,nome,password,perfil,estado")] User user)
        {
            if (ModelState.IsValid)
            {
                //testar se já existe um user com o mesmo nome
                var t = db.Users.Where(u => u.nome.Equals(user.nome)).ToList();
                if(t!=null && t.Count>0)
                {
                    ModelState.AddModelError("nome", "Já existe um utilizador com o nome igual");
                    user.perfis = new[]
                    {
                        new SelectListItem{Value="0",Text="Administrador" },
                        new SelectListItem{Value="1",Text="Utilizador"}
                    };
                    return View(user);
                }
                if (user.password==null || user.password.Trim() == "")
                {
                    ModelState.AddModelError("password", "Tem de indicar uma password");
                    user.perfis = new[]
                    {
                        new SelectListItem{Value="0",Text="Administrador" },
                        new SelectListItem{Value="1",Text="Utilizador"}
                    };
                    return View(user);
                }
                //encriptar a password
                HMACSHA512 hMACSHA512 = new HMACSHA512(new byte[] { 1 });
                var password = hMACSHA512.ComputeHash(Encoding.UTF8.GetBytes(user.password));
                user.password = Convert.ToBase64String(password);
                //   user.confirmaPassword = user.password;
                user.estado = true;
                db.Users.Add(user);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            user.perfis = new[]
            {
                new SelectListItem{Value="0",Text="Administrador" },
                new SelectListItem{Value="1",Text="Utilizador"}
            };
            return View(user);
        }

        // GET: Users/Edit/5
        [Authorize]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            //e não é admin só pode editar o seu perfil
            if (User.IsInRole("Utilizador"))
            {
                //verificar se o id é igual ao id user autenticado
                var temp = db.Users.Where(u => u.nome == User.Identity.Name && id == u.UserID).FirstOrDefault();
                if(temp==null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                user.perfis = new[]
                {
                    new SelectListItem{Value="1",Text="Utilizador"}
                };
            }
            else
            {
                user.perfis = new[]
                {
                    new SelectListItem{Value="0",Text="Administrador" },
                    new SelectListItem{Value="1",Text="Utilizador"}
                };
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Edit([Bind(Include = "UserID,nome,perfil,estado")] User user)
        {
            if (ModelState.IsValid)
            {
                var u =await db.Users.FindAsync(user.UserID);
                //se estiver a editar o user logado temos de atualizar o cookie
                if (u.nome == User.Identity.Name)
                    FormsAuthentication.SetAuthCookie(user.nome, false);

                u.nome = user.nome;
                u.perfil = user.perfil;
                u.estado = user.estado;

                db.Entry(u).State = EntityState.Modified;
                await db.SaveChangesAsync();
                if (User.IsInRole("Administrador"))
                    return RedirectToAction("Index");
                else
                    return RedirectToAction("Index", "Home");
            }
            //se não é admin só pode editar o seu perfil
            if (User.IsInRole("Utilizador"))
            {
                //verificar se o id é igual ao id user autenticado
                var temp = db.Users.Where(u => u.nome == User.Identity.Name && user.UserID == u.UserID).FirstOrDefault();
                if (temp == null)
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                user.perfis = new[]
                {
                    new SelectListItem{Value="1",Text="Utilizador"}
                };
            }
            else
            {
                user.perfis = new[]
                {
                    new SelectListItem{Value="0",Text="Administrador" },
                    new SelectListItem{Value="1",Text="Utilizador"}
                };
            }
            return View(user);
        }

        // GET: Users/Delete/5
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            User user = await db.Users.FindAsync(id);
            db.Users.Remove(user);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

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
