using Examen.Entity.Entitys.Alumno.Request;
using Examen.Entity.Entitys.Alumno.Response;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Examen.Repository.Repository.Alumno
{
    public class AlumnoRepository: BaseRepository
    {
        public async Task CreateAlumno(CreateAlumnoRequestBE model)
        {
            try
            {
                var Parameters = new SqlParameter[]
           {
                new SqlParameter{ ParameterName="@name", SqlDbType = SqlDbType.VarChar, SqlValue = model.name},
                new SqlParameter{ ParameterName="@dateOfBirth", SqlDbType = SqlDbType.DateTime, SqlValue = model.dateOfBirth},
                new SqlParameter{ ParameterName="@sex", SqlDbType = SqlDbType.VarChar, SqlValue = model.sex}
           };
                await ExecQuery("sp_create_alumno", Parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateAlumno(int id, UpdateAlumnoRequestBE model)
        {
            try
            {
                var Parameters = new SqlParameter[]
                {
                new SqlParameter{ ParameterName="@name", SqlDbType = SqlDbType.VarChar, SqlValue = model.name},
                new SqlParameter{ ParameterName="@dateOfBirth", SqlDbType = SqlDbType.DateTime, SqlValue = model.dateOfBirth},
                new SqlParameter{ ParameterName="@sex", SqlDbType = SqlDbType.VarChar, SqlValue = model.sex},
                new SqlParameter{ ParameterName="@id", SqlDbType = SqlDbType.Int, SqlValue = id}
                };
                await ExecQuery("sp_update_alumno", Parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteAlumno(int id)
        {
            try
            {
                var Parameters = new SqlParameter[]
                {
                    new SqlParameter{ ParameterName="@id", SqlDbType = SqlDbType.Int, SqlValue = id}
                };
                await ExecQuery("sp_delete_alumno", Parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<AlumnoResponseBE>> GetAlumno()
        {
            try
            {
                return await Find<AlumnoResponseBE>("sp_get_alumno");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
