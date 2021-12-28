using System;
using System.Collections.Generic;
using System.Text;

namespace Examen.Entity.Entitys.Alumno.Request
{
    public class CreateAlumnoRequestBE
    {
        public string name { get; set; }
        public DateTime dateOfBirth { get; set; }
        public string sex { get; set; }
    }
}
