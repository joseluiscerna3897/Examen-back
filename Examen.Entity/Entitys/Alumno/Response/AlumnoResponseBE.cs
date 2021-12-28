using System;
using System.Collections.Generic;
using System.Text;

namespace Examen.Entity.Entitys.Alumno.Response
{
    public class AlumnoResponseBE
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime dateOfBirth { get; set; }
        public string sex { get; set; }
        public bool state { get; set; }
    }
}
