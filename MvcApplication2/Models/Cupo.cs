using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcApplication2.Models
{
    public class Cupo
    {
        [Display(Name = "ID CUPO")]
        [StringLength(10, ErrorMessage = "Maximo de caracteres 10")]
        public int id_cupo { get; set; }
        [Display(Name = "Nombre de la materia")]
        [StringLength(10, ErrorMessage = "Maximo de caracteres 10")]
        public String MateriaString { get; set; }
        [Display(Name = "Nombre del profesor")]
        [StringLength(10, ErrorMessage = "Maximo de caracteres 10")]
        public String ProfesorString { get; set; }
        [Display(Name = "Maximo de alumnos")]
        public int maxAlumnos{ get; set; }
        [Display(Name = "Descripcion de la materia")]
        [StringLength(10, ErrorMessage = "Maximo de caracteres 10")]
        public String descripcion{ get; set; }
        public Cupo()
        {
        }

        public Cupo(String materia, string profesor, int maxAlumnos)
        {
            this.MateriaString = materia;
            this.ProfesorString = profesor;
            this.maxAlumnos = maxAlumnos;
        }
    }
}