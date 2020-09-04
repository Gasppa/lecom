using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Lecom.DAL;
using Lecom.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LECOM.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LecomController : ControllerBase
    {
        private readonly IConfiguration _config;
        public LecomController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet("DadosExames")]
        public async Task<ActionResult<List<Exame>>> GetDadosExame([FromHeader(Name = "api_key")][Required] string api_key, string codigoCliente, string codigoExame, string dataBase)
        {
            try
            {
                ExameDAL dadosExameDAL = new ExameDAL(_config);
                return await dadosExameDAL.GetDadosExames(codigoCliente, codigoExame, dataBase);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + ex.StackTrace);
            }
        }

        [HttpGet("DadosRepresentantes")]
        public async Task<ActionResult<List<Representante>>> GetDadosRepresentantes([FromHeader(Name = "api_key")][Required] string api_key, string codigoCliente)
        {
            try
            {
                RepresentanteDAL dadosRepresentanteDAL = new RepresentanteDAL(_config);
                return await dadosRepresentanteDAL.GetRepresentantes(codigoCliente);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + ex.StackTrace);
            }
        }

        [HttpPost("SalvarDesconto")]
        public async Task SalvarDesconto([FromHeader(Name = "api_key")][Required] string api_key, string codigoCliente, string descricaoConcessao, double valorDescontoFaturamento, double valorDescontoComissao )
        {
            try
            {
                DescontoClienteExameDAL dadosDesconto = new DescontoClienteExameDAL(_config);
                await dadosDesconto.SalvarDescontoClienteExame(codigoCliente, descricaoConcessao, valorDescontoFaturamento, valorDescontoComissao);
            }
            catch (Exception ex)
            {
                BadRequest(ex.Message + ex.StackTrace);
            }
        }
    }
}
