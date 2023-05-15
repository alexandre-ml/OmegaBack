using Aplicacao.Interfaces;
using Entidades.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace OmegaBackApi.Controllers
{
    /// <summary>
    /// Lançamento de horas de pessoas apoio interno
    /// </summary>

    [ApiController]
    [Route("[controller]")]
    public class HorasAiController : ControllerBase
    {
        private readonly IWHorasAiApp _horasAiApp;

        public HorasAiController(IWHorasAiApp horasAiapp)
        {
            _horasAiApp = horasAiapp;
        }

        [HttpGet]
        public async Task<ActionResult<HorasAi>> GetHorasAi()
        {
            var data = new DateTime(2023, 05, 15);

            //busca horas lançamento por chave
            //var ret = await _horasAiApp.GetList();
            
            var ret = await _horasAiApp.GetEntidadeByExpressionAsync(db => 
                db.Empresa == 46 && db.Filial == 4 && db.Pessoa == "ALEXANDREL" && db.DtLancto == data);

            return ret != null ? Ok(ret) : BadRequest("Lançamento de horas não Encontrado");


            //var ret = await _horasAiApp.GetEntidadeByExpressionAsync(db =>
            //    db.Empresa == 46 && db.Filial == 1 && db.Pessoa == "ALEXANDREL" && db.DtLancto == data);

            //return ret != null ? Ok(ret) : BadRequest("Lançamento de horas não Encontrado");
        }

        [HttpGet]
        [Route("insert")]
        public async Task<ActionResult<HorasAi>> GetHorasAiInsert()
        {
            HorasAi horas = new HorasAi
            {
                Empresa = 46,
                Filial = 4,
                Pessoa = "ALEXANDREL",
                DtLancto = new DateTime(2023, 05, 15),
                Sequencia = 1,
                Area = 3
            };

            //insere horas de lançamento pela chave
            try
            {
                await _horasAiApp.Add(horas);

                return Ok("Horas Cadastradas com Sucesso");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("update")]
        public async Task<ActionResult<HorasAi>> GetHorasAiUpDate()
        {
            HorasAi horas = new HorasAi
            {
                Empresa = 46,
                Filial = 4,
                Pessoa = "ALEXANDREL",
                DtLancto = new DateTime(2023, 05, 15),
                Sequencia = 1,
                Area = 3
            };

            //alteração de registro pela chave
            try
            {
                await _horasAiApp.Update(horas);

                return Ok("Horas Lançadas Atualizadas com Sucesso");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("delete")]
        public async Task<ActionResult<HorasAi>> GetHorasAiDelete()
        {
            HorasAi horas = new HorasAi
            {
                Empresa = 46,
                Filial = 4,
                Pessoa = "ALEXANDREL",
                DtLancto = new DateTime(2023, 05, 15),
                Sequencia = 1,
                Area = 8
            };

            //exclusão de lançamento pela chave
            try
            {
                await _horasAiApp.Delete(horas);

                return Ok("Horas Lançadas Excluida Sucesso");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
