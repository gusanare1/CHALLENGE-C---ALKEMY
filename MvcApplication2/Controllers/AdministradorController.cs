using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication2.Models;

namespace MvcApplication2.Controllers
{
    public class AdministradorController : Controller
    {
        public static readonly string[] CONTROLADORES = { "Materia","Profesor" };
        //
        // GET: /Administrador/

        public ActionResult Index(int id)
        {
            using (education_systemEntities1 db = new education_systemEntities1())
            {
                //Probando linq
                var personName = from c in db.person where c.id_person == id select c.first_name ;
                ViewBag.FirstName = personName.ToList()[0];
                return View("Index");
            }   
        }
    }
}
