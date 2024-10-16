﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
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

        #region GetSigla
        [HttpGet("{sigla}")]
        public IActionResult GetSigla([FromRoute] string sigla)
        {
            try
            {
                sigla = sigla.ToUpper();
                State state = _context.State.Find(sigla) ?? throw new NullReferenceException($"Estado com a sigla {sigla} não existe no banco de dados");
                return Ok(state);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error in GetSigla With message: {ex.Message} with path: {ex.StackTrace}");
            }
        }
        #endregion

        #region GetSiglaSmart
        [HttpGet("Search")]
        public IActionResult GetSiglaSeach([FromQuery] string value)
        {
            try
            {
                value = value.ToUpper();

                //Entity
                var state = _context.State.Where(s => s.Name.ToUpper().Contains(value) || s.Sigla.ToUpper().Contains(value)).ToList();

                /*Query criteria
                var list = from o in _context.State.ToList()
                           where o.Name.ToUpper().Contains(value) || o.Sigla.ToUpper().Contains(value)
                           select o;*/

                //Expression
                /*Expression<Func<State, bool>> stateExpression = o => true;
                stateExpression = o => o.Name.ToUpper().Contains(value) || o.Sigla.ToUpper().Contains(value);

                List<State> state = _context.State.Where(stateExpression).ToList();*/

                _ = state.FirstOrDefault() ?? throw new NullReferenceException($"Estado não existe no banco de dados");

                return Ok(state);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error in GetSiglaSearch With message: {ex.Message} with path: {ex.StackTrace}");
            }
        }
        #endregion

        #region GetSiglaPagination
        [HttpGet("Pagination")]
        public IActionResult GetStatePagination([FromQuery] string value, int skip, int take, bool desc)
        {
            try
            {
                value = value.ToUpper();

                //Entity
                //var state = _context.State.Where(s => s.Name.ToUpper().Contains(value) || s.Sigla.ToUpper().Contains(value)).ToList();

                //Query criteria
                var state = from o in _context.State.ToList()
                           where o.Name.ToUpper().Contains(value) || o.Sigla.ToUpper().Contains(value)
                           select o;

                //Expression
                /*Expression<Func<State, bool>> stateExpression = o => true;
                stateExpression = o => o.Name.ToUpper().Contains(value) || o.Sigla.ToUpper().Contains(value);*/

                //var state = _context.State.Where(stateExpression).ToList();

                _ = state.FirstOrDefault() ?? throw new NullReferenceException($"Estado não existe no banco de dados");

                if (desc)
                {
                    state = from o in state orderby o.Name descending select o;
                }
                else
                {
                    state = from o in state orderby o.Name ascending select o;
                }

                int amount = state.Count();

                state = state.Skip(skip).Take(take).ToList();

                var pr = new PaginationResponse<State>(state, amount, skip, take);

                return Ok(pr);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error in GetStatePagination With message: {ex.Message} with path: {ex.StackTrace}");
            }
        }
        #endregion

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
                state.Name = state.Name.ToUpper();
                state.Sigla = state.Sigla.ToUpper();
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
                state.Name = state.Name.ToUpper();
                state.Sigla = state.Sigla.ToUpper();
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
                return BadRequest($"Error in Put With message: {ex.Message} with path: {ex.StackTrace}");
            }
        }
        #endregion

        #region Delete
        [HttpDelete("{sigla}")]
        public IActionResult DeleteState([FromRoute] string sigla)
        {
            try
            {
                sigla = sigla.ToUpper();
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
                return BadRequest($"Error in Delete With message: {ex.Message} with path: {ex.StackTrace}");
            }
        }
        #endregion
    }
}