using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Runtime.CompilerServices;
using WFConFin.Data;
using WFConFin.Models;

namespace WFConFin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly WFConFinDbContext _context;

        public UserController(WFConFinDbContext context)
        {
            _context = context;
        }

        #region GetAll
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await _context.User.OrderBy(u => u.Name).ToListAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Post
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else if (user is null)
                {
                    return BadRequest("Usuário não pode ser nulo");
                }

                await _context.User.AddAsync(user);
                var value = await _context.SaveChangesAsync();

                if (value == 1)
                {
                    return Ok("User was insert into!");
                }
                else
                {
                    return BadRequest("User don't insert into");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Put
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] User user)
        {
            try
            {
                _context.User.Update(user);
                var value = await _context.SaveChangesAsync();

                if (value == 1)
                {
                    return Ok("Usuário foi atualizado com sucesso!");
                }
                else
                {
                    return BadRequest("Usuário não cadastrado!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Delete
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] User user)
        {
            try
            {
                _context.User.Remove(user);
                var value = await _context.SaveChangesAsync();

                if (value == 1)
                {
                    return Ok("Usuário foi deletado com sucesso!");
                }
                else
                {
                    return BadRequest("Usuário não foi deletado");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}
