using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication2.Models;
using System.Globalization;
using System.Diagnostics;
namespace MvcApplication2.Controllers
{
    public class ProfesorController : Controller
    {

        public ProfesorController()
        {
            
        }


        // GET: /Profesor/
        public ActionResult Index()
        {
            using (education_systemEntities1 db = new education_systemEntities1())
            {
                var er = db.business_position.Where(m => m.position.position_name== "Profesor").First();
                var profesores = from c in db.person
                                 join d in db.person_business_position on c.id_person equals d.person_id
                                 where d.business_position_id == er.id_business_position
                                 select c;
                if (TempData["Message"] != null)
                {
                    ViewBag.Message = TempData["Message"];
                    TempData.Remove("Message");
                }
                return View("Index", profesores.ToList());
            }
        }


        //GET: /Materia/Create/
        [HttpGet, ActionName("Create")]
        public ActionResult CreateGet()
        {
            return View("Create");
        }


        //POST: Materia/Create/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create()
        {
            using (education_systemEntities1 db = new education_systemEntities1())
            {
                var er = db.business_position.Where(m => m.position.position_name == "Profesor").First();

                person p = new person();
                p.id_person = 0;
                p.first_name = Request.Form["first_name"];
                p.middle_name = Request.Form["middle_name"];
                p.last_name = Request.Form["last_name"];
                p.second_last_name = Request.Form["second_last_name"];
                p.fecha_nacimiento = Convert.ToDateTime(Request.Form["fecha_nacimiento"]);
                p.cedula = Request.Form["cedula"];
                
                db.person.Add(p);
                db.SaveChanges();

                var pe = db.person.Where(m => m.cedula == p.cedula).First();
                person_business_position per = new person_business_position();
                per.person_id = pe.id_person;
                per.business_position_id = er.id_business_position;
                per.username = Request.Form["login"];
                per.pwd = Request.Form["login"];
                
                db.person_business_position.Add(per);
                db.SaveChanges();

                TempData["Message"] = "Create Sucess!";
                return RedirectToAction("Index");

            }

            
        }

        

        //GET: /Profesor/Edit/
        public ActionResult Edit(int id)
        {
            using (education_systemEntities1 db = new education_systemEntities1())
            {
                person prof = new person();
                
                var profesor = db.person.Where(m => m.id_person == id).First<person>();
                var bp = db.business_position.Where(m=>m.position.position_name == "Profesor").First();
                prof.id_person = id;
                prof.last_name = profesor.last_name;
                prof.first_name = profesor.first_name;
                prof.middle_name = profesor.middle_name;
                prof.second_last_name = profesor.second_last_name;
                Debug.WriteLine(profesor.fecha_nacimiento.Date); //    5/8/2020 0:00:00


                
                //DateTime dt = DateTime.ParseExact(profesor.fecha_nacimiento.ToString(), "dd/M/yyyy h:mm:ss", CultureInfo.InvariantCulture);
                //string s = dt.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);


                //prof.fecha_nacimiento = DateTime.ParseExact(s, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                prof.fecha_nacimiento = profesor.fecha_nacimiento.Date;

                prof.cedula = profesor.cedula;
                
                //var profesor_pbp = from c in db.person_business_position where c.person_id == profesor.id_person select c;
                var profesor_pbp = db.person_business_position.Where(m => m.person_id == profesor.id_person && m.business_position_id == bp.id_business_position ).ToList();//
                profesor.person_business_position = profesor_pbp;
                prof.person_business_position = profesor_pbp;
                return View("Edit", prof);
            }

        }

        //POST: /Profesor/Edit/
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost()
        {
            using (education_systemEntities1 db = new education_systemEntities1())
            {
                int id_person = Int32.Parse(Request.Form["id_person"]);

                String intEstado = Request.Form["estado"].ToString();
                String strEstado = null;
                if(intEstado == "1")
                    strEstado = "Activo";
                else 
                    strEstado = "Inactivo";

                person p = db.person.Find(id_person);

                p.first_name = Request.Form["first_name"];
                p.middle_name = Request.Form["middle_name"];
                p.last_name = Request.Form["last_name"];
                p.second_last_name = Request.Form["second_last_name"];
                p.fecha_nacimiento = DateTime.ParseExact(Request.Form["fecha_nacimiento"], "yyyy-MM-dd", CultureInfo.InvariantCulture);

                p.cedula = Request.Form["cedula"];
                
                var pbp = db.person_business_position.Where(m => m.person_id == p.id_person).First();
                pbp.username = Request.Form["login"];
                pbp.pwd = Request.Form["pwd"];
                pbp.estado = strEstado;

                db.Entry(pbp).State = System.Data.EntityState.Modified;
                db.Entry(p).State = System.Data.EntityState.Modified;
                db.SaveChanges();
                TempData["Message"] = "Edit Sucess!";
            }
            return RedirectToAction("Index");
        }

        //GET: /Materia/Details/
        public ActionResult Details(int id)
        {
            using (education_systemEntities1 db = new education_systemEntities1())
            {
                var s = db.person.Find(id);
                return View("Details", s);
            }
        }

        /*
        [HttpGet, ActionName("Delete")]
        public ActionResult DeleteGet(int id)
        {
            using (education_systemEntities1 db = new education_systemEntities1())
            {
                var s = db.person.Find(id);
                return View("Delete", s);
            }
        }

        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            using (education_systemEntities1 db = new education_systemEntities1())
            {
                var s = db.person.Find(id);
                db.person.Remove(s);
                db.SaveChanges();
                TempData["Message"]= "Delete Sucess";
                return RedirectToAction("Index");
            }
        }
        */





    }
}
