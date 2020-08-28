using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcApplication2.Controllers;
namespace MvcApplication2.Filters
{
    public class Filtro : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            String controller = filterContext.RouteData.Values["controller"].ToString();
            Debug.WriteLine(controller);
            if (HttpContext.Current.Session["id_person"] == null && (controller != "Login"))
            {
                filterContext.Controller.TempData["Message"] = "No ha iniciado sesion";
                filterContext.Result = new RedirectToRouteResult(
                  new RouteValueDictionary{
                    {"Controller", "Login"},
                    {"Action", "Index"}
            });
            }
            
            if ( AdministradorController.CONTROLADORES.Contains(controller) ) //Si estas en el controladr Administrador
            {
                Debug.WriteLine(controller);
                try
                {
                    Debug.WriteLine(HttpContext.Current.Session["admin"].ToString());
                }
                catch (NullReferenceException) //No tiene administrador en sesion
                {
                    Debug.WriteLine("Noteine permisos");
                    filterContext.Controller.TempData["Message"] = "No tiene permisos";
                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary{
                            {"Controller", "Login"},
                            {"Action", "Logout"}
                            });
                }
            }


            if (AlumnoController.CONTROLADORES.Contains(controller)) //Si estas en el controladr Alumno
            {
                try
                {
                    Debug.WriteLine(HttpContext.Current.Session["alumno"].ToString());
                }
                catch (NullReferenceException) //No tiene administrador en sesion
                {
                    Debug.WriteLine("Noteine permisos");
                    filterContext.Controller.TempData["Message"] = "No tiene permisos";
                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary{
                            {"Controller", "Login"},
                            {"Action", "Logout"}
                            });
                }

            }   
            base.OnActionExecuting(filterContext);
        }
    }
}