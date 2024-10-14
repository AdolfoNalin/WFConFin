using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using System.Resources;
using WFConFin.Data;
using WFConFin.Models;

namespace WFConFin.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class StateController : Controller
    {
        private readonly WFConFinDbContext _context;

        public StateController(WFConFinDbContext context)
        {
            this._context = context;
        }

        #region GetAll
        [HttpGet]
        public IActionResult GetStates()
        {
            try
            {
                return Ok(_context.State.OrderBy(s => s.Name).ToList<State>());
            }
            catch (Exception ex)
            {
                return BadRequest($"Error in List to type get, with message: {ex.Message} with path: {ex.StackTrace}");
            }
        }
        #endregion

        #region Post
        [HttpPost]
        public IActionResult PostState([FromBody] State state)
        {
            try
            {
                _ = state ?? throw new NullReferenceException("Por favor preencha o Todos os dados do Estado!");
                _context.State.Add(state);
                int value = _context.SaveChanges();

                if (value == 0)
                {
                    return BadRequest("Error State não cadastrado");
                }

                return Ok("Sucesso, Estado Incluido!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error in Post With message: {ex.Message} with path: {ex.StackTrace}");
            }
        }
        #endregion

        #region Put
        [HttpPut]
        public IActionResult PutState([FromBody] State state)
        {
            try
            {
                _ = state ?? throw new NullReferenceException("Por favor preencha o Todos os dados do Estado!");
                _context.State.Update(state);
                int value = _context.SaveChanges();

                if (value == 0)
                {
                    return BadRequest("Error estado não alterado");
                }

                return Ok("Sucesso, Estado alterado!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error in Post With message: {ex.Message} with path: {ex.StackTrace}");
            }
        }
        #endregion

        #region Delete
        [HttpDelete("{sigla}")]
        public IActionResult DeleteState([FromRoute] string sigla)
        {
            try
            {
                State state = _context.State.Find(sigla) ?? throw new NullReferenceException("Estado não encotrado");

                _context.State.Remove(state);

                if(_context.SaveChanges() == 0)
                {
                    return BadRequest("Estado não excluido!");
                }

                return Ok("Estado excluido com sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error in Post With message: {ex.Message} with path: {ex.StackTrace}");
            }
        }
        #endregion
    }
}