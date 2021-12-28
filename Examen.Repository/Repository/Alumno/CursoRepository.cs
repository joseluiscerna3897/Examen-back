using Examen.Entity.Entitys.Alumno.Response;
using Examen.Entity.Entitys.Curso.Request;
using Examen.Entity.Entitys.Curso.Response;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Examen.Repository.Repository.Alumno
{
    public class CursoRepository : BaseRepository
    {
        public async Task CreateCurso(CreateCursoRequestBE model)
        {
            try
            {
                var Parameters = new SqlParameter[]
                {
                    new SqlParameter{ ParameterName="@description", SqlDbType = SqlDbType.VarChar, SqlValue = model.description},
                };
                await ExecQuery("sp_create_curso", Parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateCurso(int id, UpdateCursoRequestBE model)
        {
            try
            {
                var Parameters = new SqlParameter[]
                {
                    new SqlParameter{ ParameterName="@description", SqlDbType = SqlDbType.VarChar, SqlValue = model.description},
                    new SqlParameter{ ParameterName="@id", SqlDbType = SqlDbType.Int, SqlValue = id}
                };
                await ExecQuery("sp_update_curso", Parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<CursoResponseBE>> GetCurso()
        {
            try
            {
                return await Find<CursoResponseBE>("sp_get_curso");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
