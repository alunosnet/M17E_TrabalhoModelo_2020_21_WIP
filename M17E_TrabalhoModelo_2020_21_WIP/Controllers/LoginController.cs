using M17E_TrabalhoModelo_2020_21_WIP.Data;
using M17E_TrabalhoModelo_2020_21_WIP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace M17E_TrabalhoModelo_2020_21_WIP.Controllers
{
    public class LoginController : Controller
    {
        private M17E_TrabalhoModelo_2020_21_WIPContext db = new M17E_TrabalhoModelo_2020_21_WIPContext();

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(User user)
        {
            if(user.nome!=null && user.password != null)
            {
                //hash password
                //encriptar a password
                HMACSHA512 hMACSHA512 = new HMACSHA512(new byte[] { 1 });
                var password = hMACSHA512.ComputeHash(Encoding.UTF8.GetBytes(user.password));
                user.password = Convert.ToBase64String(password);
                //consulta a bd 
                foreach(var utilizador in db.Users.ToList())
                {
                    //se existir o utilizador
                    if(utilizador.nome==user.nome && utilizador.password == user.password)
                    {
                        //iniciar a sessão
                        FormsAuthentication.SetAuthCookie(user.nome, false);
                        //redirecionar o user para a ação que estava a tentar executar, caso exista!
                        if (Request.QueryString["ReturnUrl"] == null)
                            return RedirectToAction("Index", "Home");
                        else
                            return Redirect(Request.QueryString["ReturnUrl"].ToString());
                    }
                }
            }
            ModelState.AddModelError("", "Login falhou. Tente novamente.");
            return View(user);
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