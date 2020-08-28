using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication2.Models;
namespace MvcApplication2.Controllers
{
    public class AlumnoController : Controller
    {
        public static readonly string[] CONTROLADORES = { "Alumno"};
        //
        // GET: /Alumno/

        public ActionResult Index()
        {
            using (education_systemEntities1 db = new education_systemEntities1())
            {
                var q = db.quota.Where(m=>m.alumno_id!=null).ToList<quota>();
                foreach (var r in q)
                {
                    r.subject = db.subject.Find(r.subject_id);
                }
               // var q = from c in db.quota where c.alumno_id != null select c;
                
                return View("Index",q);
            }
        }

        // GET: /Alumno/Create
        public ActionResult Create()
        {
            using (education_systemEntities1 db = new education_systemEntities1())
            {
                var listaq = db.quota.Where(m=>m.alumno_id==null && m.subject.max_alumnos>0).OrderBy(c=>c.subject.codigo_subject).ToList();
                
                List<Cupo> listaCupo = new List<Cupo>();
                foreach (quota q in listaq)
                {
                    var profesor = db.person.Find(q.profesor_id);

                    Cupo c = new Cupo();
                    c.id_cupo = q.id_quota;
                    c.MateriaString = q.subject.nombre_subject;
                    c.ProfesorString = profesor.first_name+" "+ profesor.last_name;
                    c.maxAlumnos = q.subject.max_alumnos;
                    c.descripcion = q.subject.descripcion;
                    listaCupo.Add(c);
                }
                return View("Create",listaCupo);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePost()
        {
            
            using (education_systemEntities1 db = new education_systemEntities1())
            {
                int id_quota = Int32.Parse(Request.Form["id_quota"]);
                quota q = new quota();
                
                var qt = db.quota.Find(id_quota);
                qt.subject.max_alumnos = qt.subject.max_alumnos - 1;
                q.subject_id = qt.subject_id;
                q.alumno_id  = Int32.Parse(HttpContext.Session["id_person"].ToString());
                q.profesor_id = qt.profesor_id;
                
                db.quota.Add(q);
                db.Entry(qt).State = System.Data.EntityState.Modified;
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            
        }

    }
}
