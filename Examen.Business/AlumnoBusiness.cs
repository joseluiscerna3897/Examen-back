using Examen.Entity.Entitys.Alumno.Request;
using Examen.Entity.Entitys.Alumno.Response;
using Examen.Repository.Repository.Alumno;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Examen.Business
{
    public class AlumnoBusiness
    {
        AlumnoRepository _repository = new AlumnoRepository();


        public async Task<List<AlumnoResponseBE>> getAlumno()
        {
            return await _repository.GetAlumno();
        }

        public async Task CreateAlumno(CreateAlumnoRequestBE model)
        {
            await _repository.CreateAlumno(model);
        }

        public async Task UpdateAlumno(int id, UpdateAlumnoRequestBE model)
        {
            await _repository.UpdateAlumno(id, model);
        }

        public async Task DeleteAlumno(int id)
        {
            await _repository.DeleteAlumno(id);
        }
    }
}
