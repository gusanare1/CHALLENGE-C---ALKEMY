using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication2.Models;
using System.Diagnostics;
namespace MvcApplication2.Controllers
{
    public class LoginController : Controller
    {
        // GET: /login
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult loginPost()
        {
            String username = Request.Form["username"];
            String pwd = Request.Form["pwd"];
            Debug.WriteLine("Logueando");
            using (education_systemEntities1 db = new education_systemEntities1())
            {
                try
                {
                    var pbp = db.person_business_position.Where(d => d.username == username).Where(d=> d.pwd == pwd).First();
                    HttpContext.Session["id_person"] = pbp.person_id;
                    int id_position = pbp.business_position.position.id_position;
                    String nombre_rol = pbp.business_position.position.position_name;
                    return RedirectToAction("Bienvenido", new { id_rol = id_position });
                }
                catch (InvalidOperationException) //Null NO hay login registrado
                {
                    return RedirectToAction("Error", new { id = 2 });
                }
            }
        }

        public ActionResult Index()
        {
            if (TempData["Message"] != null)
            {
                Debug.WriteLine(TempData["Message"]);
                ViewBag.Message = TempData["Message"];
                TempData.Remove("Message");
            }
            return View("Index");
        }


        public ActionResult Bienvenido(int id_rol)
        {
            try
            {
                //*Poco uso con sesiones
                int idPerson = Int32.Parse(HttpContext.Session["id_person"].ToString()); //Saber si tengo una persona en sesion...

                if (id_rol == 1) //rol = 1 : Alumno
                {
                    HttpContext.Session["alumno"] = "1";

                    return RedirectToAction("Index", "Alumno", new { id = idPerson });
                }
                else //rol = 2: Administrador
                {
                    HttpContext.Session["admin"] = "1";
                    return RedirectToAction("Index", "Administrador", new { id = idPerson });
                }
            }
            catch (ArgumentNullException)
            {
                  return RedirectToAction("Error", new { id = 3 });
            }
        }

        public ActionResult Error(int id)
        {
            TempData["Message"] = "Usuario/password incorrecto";
            return RedirectToAction("Index");
            //return "Error: "+Convert.ToString(id);
        }

        // GET: /logout
        public ActionResult Logout()
        {
            
            HttpContext.Session["id_person"] = null;
            return RedirectToAction("Index");
        }

    }
}