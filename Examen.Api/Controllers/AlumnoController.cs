using Examen.Business;
using Examen.Entity;
using Examen.Entity.Entitys.Alumno.Request;
using Examen.Entity.Entitys.Alumno.Response;
using Examen.Utilities.General;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Examen.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlumnoController : ControllerBase
    {
        AlumnoBusiness _business = new AlumnoBusiness();
        private readonly IConfiguration _configuration;

        public AlumnoController(IConfiguration configuration)
        {
            _configuration = configuration;
            GeneralModel.ConnectionString = _configuration["ConnectionStrings:EXAMEN"];
        }

        [HttpPost]
        public async Task<IActionResult> CreateAlumno([FromBody] CreateAlumnoRequestBE model)
        {
            try
            {
                Response response = new Response();
                await _business.CreateAlumno(model);
                response.Code = (int)Enums.StoreCode.OK;
                response.Message = Constans.MESSAGE_SAVE;
                return Ok(response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAlumno(int id, [FromBody] UpdateAlumnoRequestBE model)
        {
            try
            {
                Response response = new Response();
                await _business.UpdateAlumno(id, model);
                response.Code = (int)Enums.StoreCode.OK;
                response.Message = Constans.MESSAGE_UPDATE;
                return Ok(response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut("delete/{id}")]
        public async Task<IActionResult> DeleteAlumno(int id)
        {
            try
            {
                Response response = new Response();
                await _business.DeleteAlumno(id);
                response.Code = (int)Enums.StoreCode.OK;
                response.Message = Constans.MESSAGE_DELETE;
                return Ok(response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet()]
        public async Task<IActionResult> GetAlumno()
        {
            try
            {
                Response response = new Response();
                List<AlumnoResponseBE> almacen = await _business.getAlumno();
                response.Code = (int)Enums.StoreCode.OK;
                response.Data = almacen;
                return Ok(response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
