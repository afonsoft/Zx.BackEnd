using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Zx.BackEnd.Model;

namespace Zx.BackEnd.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PDVController : ControllerBase
    {

        private readonly Core.PDVBusiness pdvBO;

        public PDVController()
        {
            pdvBO = new Core.PDVBusiness();
        }

        /// <summary>
        /// Listar todos os Pontos de Vendas
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(PontoDeVendas), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<PontoDeVendas> GetPDV()
        {
            IList<PontoDeVenda> listaPdvs = pdvBO.Get().ToList();
            if (listaPdvs.Count > 0)
            {
                return Ok(new PontoDeVendas { pdvs = listaPdvs });
            }
            return NoContent();
        }

        /// <summary>
        /// Lista o Ponto de Venda pelo Id
        /// </summary>
        /// <param name="id">Id do ponto de vendas</param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PontoDeVendas), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<PontoDeVendas> GetPDV(int id)
        {
            var pdv = pdvBO.GetById(id);
            if (pdv != null)
            {
                return Ok(new PontoDeVendas{ pdvs = new List<PontoDeVenda>{ pdv } });
            }
            return NotFound(id);
        }

        /// <summary>
        /// Localizar os pontos de vendas mais proximos
        /// </summary>
        /// <param name="latitude">latitude</param>
        /// <param name="longitude">longitude</param>
        [HttpGet("{latitude}/{longitude}")]
        [ProducesResponseType(typeof(PontoDeVendas), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<PontoDeVendas> GetPDV(double latitude, double longitude)
        {
            IList<PontoDeVenda> listaPdvs = pdvBO.Seach(latitude, longitude).ToList();
            if (listaPdvs.Count > 0)
            {
                return Ok(new PontoDeVendas { pdvs = listaPdvs });
            }
            return NotFound();
        }


        /// <summary>
        /// Criar um ponto de Venda
        /// </summary>
        /// <param name="pdv"></param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult CreatePDV([FromBody] PontoDeVenda pdv)
        {
            var cnpj = pdvBO.Add(pdv);
            if (cnpj != null)
            {
                return Accepted();
            }

            return Conflict("CNPJ já existente");
        }

        /// <summary>
        /// Deletar um ponto de Venda
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult DeletePDV(int id)
        {
            if (pdvBO.Delete(id))
                return Accepted();
            else
                return Conflict($"Id {id} inválido.");


        }
    }
}
