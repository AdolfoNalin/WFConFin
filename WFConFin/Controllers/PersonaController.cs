using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Xml;
using WFConFin.Data;
using WFConFin.Models;

namespace WFConFin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonaController : Controller
    {
        private readonly WFConFinDbContext _context;

        public PersonaController(WFConFinDbContext context)
        {
            _context = context;
        }

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

        #region Get
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
                
                Persona value = _context.Persona.Find(persona.Id) ?? throw new NullReferenceException("Pessoa não encontrada!");

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
            catch(NullReferenceException ne)
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

                if(value == 1)
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
