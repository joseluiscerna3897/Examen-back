using Examen.Entity.Entitys.Curso.Request;
using Examen.Entity.Entitys.Curso.Response;
using Examen.Repository.Repository.Alumno;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Examen.Business
{
    public class CursoBusiness
    {
        CursoRepository _repository = new CursoRepository();


        public async Task<List<CursoResponseBE>> getCurso()
        {
            return await _repository.GetCurso();
        }

        public async Task CreateCurso(CreateCursoRequestBE model)
        {
            await _repository.CreateCurso(model);
        }

        public async Task UpdateCurso(int id, UpdateCursoRequestBE model)
        {
            await _repository.UpdateCurso(id, model);
        }
    }
}
