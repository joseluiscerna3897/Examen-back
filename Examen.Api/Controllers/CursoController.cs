using Examen.Business;
using Examen.Entity;
using Examen.Entity.Entitys.Curso.Request;
using Examen.Entity.Entitys.Curso.Response;
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
    public class CursoController : ControllerBase
    {
        CursoBusiness _business = new CursoBusiness();
        private readonly IConfiguration _configuration;

        public CursoController(IConfiguration configuration)
        {
            _configuration = configuration;
            GeneralModel.ConnectionString = _configuration["ConnectionStrings:EXAMEN"];
        }

        [HttpPost]
        public async Task<IActionResult> CreateCurso([FromBody] CreateCursoRequestBE model)
        {
            try
            {
                Response response = new Response();
                await _business.CreateCurso(model);
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
        public async Task<IActionResult> UpdateCurso(int id, [FromBody] UpdateCursoRequestBE model)
        {
            try
            {
                Response response = new Response();
                await _business.UpdateCurso(id, model);
                response.Code = (int)Enums.StoreCode.OK;
                response.Message = Constans.MESSAGE_UPDATE;
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
                List<CursoResponseBE> almacen = await _business.getCurso();
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
