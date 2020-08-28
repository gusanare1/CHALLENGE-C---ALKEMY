using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication2.Models;
using System.Web.Routing;

namespace MvcApplication2.Controllers
{
    public class MateriaController : Controller
    {
       
       

        //
        // GET: /Materia/
        public ActionResult Index()
        {

            using (education_systemEntities1 db = new education_systemEntities1())
            {
                var materias = db.subject.ToList();
                if (TempData["Message"] != null)
                {
                    ViewBag.Message = TempData["Message"];
                    TempData.Remove("Message");
                }
                return View("Index", materias);
            }


        }

        //POST: Materia/Create/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePost()
        {
            using (education_systemEntities1 db = new education_systemEntities1())
            {
                int profesor_id = Int32.Parse(Request.Form["profesor_id"]);
                quota q = new quota();
                subject s = new subject();
                s.id_subject = 1;
                s.nombre_subject = Request.Form["nombre_subject"];
                s.codigo_subject = Request.Form["codigo_subject"];
                s.max_alumnos = Int32.Parse(Request.Form["max_alumnos"]);
                s.descripcion = Request.Form["descripcion"];
                s.hora_entrada = TimeSpan.Parse(Request.Form["hora_entrada"]);
                s.hora_salida = TimeSpan.Parse(Request.Form["hora_salida"]);
                db.subject.Add(s);
                db.SaveChanges();
                
                s = db.subject.Where(m=>m.codigo_subject==s.codigo_subject).First();
                q.profesor_id = profesor_id;
                q.subject_id = s.id_subject;
                q.periodo = DateTime.Now.Year.ToString();
                db.quota.Add(q);
                db.SaveChanges();

                TempData["Message"] = "Create sucess!";
            }

            return RedirectToAction("Index");
        }

        //GET: /Materia/Create/
        public ActionResult Create()
        {
            using (education_systemEntities1 db = new education_systemEntities1())
            {
                subject materia = new subject();
                
                var profesores = from c in db.business_position
                                 join d in db.person_business_position on c.id_business_position equals d.business_position_id
                                 join e in db.person on d.person_id equals e.id_person
                                 where c.position.position_name == "Profesor"
                                 select e;


                materia.profesores = profesores.ToList();
                return View("Create", materia);
            }
        }

        //GET: /Materia/Edit/
        public ActionResult Edit(int id)
        {
            using (education_systemEntities1 db = new education_systemEntities1())
            {
                var materia = db.subject.Where(m=>m.id_subject==id).First();
                var profesores = from c in db.business_position
                                 join d in db.person_business_position on c.id_business_position equals d.business_position_id
                                 join e in db.person on d.person_id equals e.id_person
                                 where c.position.position_name == "Profesor"
                                 select e;

                var q = db.quota.Where(m => m.subject_id == id).First();
                materia.profesores = profesores.ToList();
                materia.quota.ToList()[0].id_quota = q.id_quota;
                return View("Edit", materia);
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost()
        {
            using (education_systemEntities1 db = new education_systemEntities1())
            {
                int id_subject = Int32.Parse(Request.Form["id_subject"]);
                int id_quota= Int32.Parse(Request.Form["id_quota"]);

                subject s = db.subject.Find(id_subject);
                //var su = db.subject.Find(id);


                int profesor_id = Int32.Parse(Request.Form["profesor_id"]);
                
                quota q = db.quota.Where(m=>m.id_quota==id_quota).First();
                
                s.nombre_subject = Request.Form["nombre_subject"];
                s.codigo_subject = Request.Form["codigo_subject"];
                s.max_alumnos = Int32.Parse(Request.Form["max_alumnos"]);
                s.descripcion = Request.Form["descripcion"];
                s.hora_entrada = TimeSpan.Parse(Request.Form["hora_entrada"]);
                s.hora_salida = TimeSpan.Parse(Request.Form["hora_salida"]);
                
                                
                q.profesor_id = profesor_id;
                q.subject_id = s.id_subject;
                q.periodo = DateTime.Now.Year.ToString();
                

                db.Entry(s).State = System.Data.EntityState.Modified;
                db.Entry(q).State = System.Data.EntityState.Modified;
                db.SaveChanges();
                TempData["Message"]= "Edit sucess!";
            }
            return RedirectToAction("Index");
        }

        //GET: /Materia/Details/
        public ActionResult Details(int id)
        {
            using (education_systemEntities1 db = new education_systemEntities1())
            {
                var s= db.subject.Find(id);
                return View("Details",s);
            }
        }


    }
}

