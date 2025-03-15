using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WFConFin.Data;
using WFConFin.Models;

namespace WFConFin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PersonaController : Controller
    {
        private readonly WFConFinDbContext _context;

        public PersonaController(WFConFinDbContext context)
        {
            _context = context;
        }

        #region GetId
        [HttpGet("{id}")]
        public async Task<IActionResult> GetName([FromRoute] Guid id)
        {
            try
            {
                Persona persona = await _context.Persona.FindAsync(id) ?? throw new NullReferenceException($"Pessoa com o id {id} não existe no banco de dados");
                return Ok(persona);
            }
            catch (NullReferenceException ne)
            {
                return NotFound(ne.Message); 
            }
            catch (Exception ex)
            {
                return BadRequest($"Error in GetName With message: {ex.Message} with path: {ex.StackTrace}");
            }
        }
        #endregion

        #region GetNameSmart
        [HttpGet("Name")]
        public async Task<IActionResult> GetNameSeach([FromQuery] string value)
        {
            try
            {
                value = value.ToUpper();

                var persona = await _context.Persona.Where(s => s.Name.ToUpper().Contains(value) || s.Email.ToUpper().Contains(value)).ToListAsync();

                _ = persona.FirstOrDefault() ?? throw new NullReferenceException($"Objeto não existe no banco de dados");

                return Ok(persona);
            }
            catch (NullReferenceException ne)
            {
                return BadRequest(ne.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error in GetNameSearch With message: {ex.Message} with path: {ex.StackTrace}");
            }
        }
        #endregion

        #region GetPersonaPagination
        [HttpGet("Pagination")]
        public async Task<IActionResult> GetPersonaPagination([FromQuery] string? value, int skip, int take, bool desc)
        {
            try
            {

                var persona = from o in await _context.Persona.ToListAsync() select o;

                if(!value.IsNullOrEmpty())
                {
                    value = value.ToUpper();
                    persona = from o in persona where o.Name.ToUpper().Contains(value) || o.Email.ToUpper().Contains(value)
                    select o;
                }

                _ = persona.FirstOrDefault() ?? throw new NullReferenceException($"Pessoa não existe no banco de dados");

                if (desc)
                {
                    persona = from o in persona orderby o.Name descending select o;
                }
                else
                {
                    persona = from o in persona orderby o.Name ascending select o;
                }

                int amount = persona.Count();

                persona = persona.Skip((--skip) * take).Take(take).ToList();

                var pr = new PaginationResponse<Persona>(persona, amount, skip, take);

                return Ok(pr);
            }
            catch (NullReferenceException ne)
            {
                return BadRequest(ne.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error in GetStatePagination With message: {ex.Message} with path: {ex.StackTrace}");
            }
        }
        #endregion

        #region Post
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Persona persona)
        {
            try
            {
                persona.BirthDate = persona.BirthDate.ToUniversalTime();

                await _context.Persona.AddAsync(persona);
                var value = await _context.SaveChangesAsync();

                if (value == 1)
                {
                    return Ok("Cadastro realizado com sucesso!");
                }
                else
                {
                    return BadRequest("A pessoa não foi cadastrada");
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message} {ex.StackTrace}");
            }
        }
        #endregion

        #region GetAll
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await _context.Persona.OrderBy(p => p.Name).ToListAsync());
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}: {ex.StackTrace}");
            }
        }
        #endregion

        #region Put
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Persona persona)
        {
            try
            {
                if (persona is null)
                {
                    throw new NullReferenceException("O objeto não pode ser nulo!");
                }

                persona.BirthDate = persona.BirthDate.ToUniversalTime();

                _context.Persona.Update(persona);

                var SaveReuslt = await _context.SaveChangesAsync();

                if (SaveReuslt == 1)
                {
                    return Ok("Atualização cadastrado com sucesso!");
                }
                else
                {
                    return BadRequest("Infelizmente a atualização não foi realizada!");
                }
            }
            catch (NullReferenceException ne)
            {
                return BadRequest(ne.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}: {ex.StackTrace}");
            }
        }
        #endregion

        #region Delete
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid Id)
        {
            try
            {
                Persona persona = _context.Persona.Find(Id) ?? throw new NullReferenceException("Pessoa não encontrada!");

                _context.Persona.Remove(persona);

                var value = await _context.SaveChangesAsync();

                if (value == 1)
                {
                    return Ok("Deletado com sucesso!");
                }
                else
                {
                    return BadRequest("A deleção não deu certo!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}: {ex.StackTrace}");
            }
        }
        #endregion
    }
}
