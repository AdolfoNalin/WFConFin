﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Resources;
using System.Xml.Linq;
using WFConFin.Data;
using WFConFin.Models;

namespace WFConFin.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    [Authorize]
    public class StateController : Controller
    { 
        private readonly WFConFinDbContext _context;

        public StateController(WFConFinDbContext context)
        {
            this._context = context;
        }

        #region GetSigla
        [HttpGet("{sigla}")]
        public async Task<IActionResult> GetSigla([FromRoute] string sigla)
        {
            try
            {
                sigla = sigla.ToUpper();
                State state = await _context.State.FindAsync(sigla) ?? throw new NullReferenceException($"Estado com a sigla {sigla} não existe no banco de dados");
                return Ok(state);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error in GetSigla With message: {ex.Message} with path: {ex.StackTrace}");
            }
        }
        #endregion

        #region GetSmart
        [HttpGet("Search")]
        public async Task<IActionResult> GetSiglaSeach([FromQuery] string value)
        {
            try
            {
                value = value.ToUpper();

                //Entity
                var state = await _context.State.Where(s => s.Name.ToUpper().Contains(value) || s.Sigla.ToUpper().Contains(value)).OrderBy(s => s.Name).ToListAsync<State>() 
                    ?? throw new NullReferenceException($"Estado não existe no banco de dados");

                return Ok(state);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error in GetSiglaSearch With message: {ex.Message} with path: {ex.StackTrace}");
            }
        }
        #endregion

        #region GetPagination
        [HttpGet("Pagination")]
        public async  Task<IActionResult> GetPagination([FromQuery] string? value, int skip, int take, bool desc)
        {
            try
            {
                var state = await _context.State.ToListAsync();

                _ = state.FirstOrDefault() ?? throw new NullReferenceException($"Estado não existe no banco de dados");

               
                if (!value.IsNullOrEmpty())
                {
                    //value = value.ToUpper();
                    state = state.Where(s => s.Name.ToUpper().Contains(value) || s.Sigla.ToUpper().Contains(value)).ToList();
                }

                if (desc)
                {
                    state = state.OrderByDescending(s => s.Name).ToList();
                }
                else
                {
                    state = state.OrderBy(s => s.Name).ToList();
                }

                int amount = state.Count();

                state = state.Skip((--skip) * take).Take(take).ToList();

                var pr = new PaginationResponse<State>(state, amount, skip, take);

                return Ok(pr);
            }
            catch (NullReferenceException ne)
            {
                return NotFound(ne.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error in GetStatePagination With message: {ex.Message} with path: {ex.StackTrace}");
            }
        }
        #endregion

        #region GetAll
        [HttpGet]
        public async Task<IActionResult> GetStates()
        {
            try
            {
                List<State> list = new List<State>();
                list = await _context.State.OrderBy(s => s.Name).ToListAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error in List to type get, with message: {ex.Message} with path: {ex.StackTrace}");
            }
        }
        #endregion

        #region Post
        [HttpPost]
        public async Task<IActionResult> PostState([FromBody] State state)
        {
            try
            {
                state.Name = state.Name.ToUpper();
                state.Sigla = state.Sigla.ToUpper();
                _ = state ?? throw new NullReferenceException("Por favor preencha o Todos os dados do Estado!");
                await _context.State.AddAsync(state);
                int value = await _context.SaveChangesAsync();

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
        public async Task<IActionResult> PutState([FromBody] State state)
        {
            try
            {
                state.Name = state.Name.ToUpper();
                state.Sigla = state.Sigla.ToUpper();
                _ = state ?? throw new NullReferenceException("Por favor preencha o Todos os dados do Estado!");
                _context.State.Update(state);
                int value = await _context.SaveChangesAsync();

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
        public async Task<IActionResult> DeleteState([FromRoute] string sigla)
        {
            try
            {
                sigla = sigla.ToUpper();
                State state = await _context.State.FindAsync(sigla) ?? throw new NullReferenceException("Estado não encotrado");

                _context.State.Remove(state);

                if(await _context.SaveChangesAsync() == 0)
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