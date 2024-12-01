using Microsoft.AspNetCore.Mvc;
using WFConFin.Data;
using WFConFin.Models;

namespace WFConFin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private WFConFinDbContext _context;

        public AccountController(WFConFinDbContext context)
        {
            _context = context;
        }

        #region GetAll
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(_context.Account.OrderBy(x => x.Id).ToList<Account>());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Post
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Account account)
        {
            try
            {
                await _context.Account.AddAsync(account ?? throw new NullReferenceException("Conta não pode ser nulo!"));
                var value = await _context.SaveChangesAsync();

                if (value == 1)
                {
                    return Ok("Conta cadastrada com suscesso!");
                }
                else
                {
                    return BadRequest("Conta não cadastrada!");
                }
            }
            catch (NullReferenceException ne)
            {
                return NotFound(ne.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Put
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Account account)
        {
            try
            {
                _context.Account.Update(account ?? throw new NullReferenceException("Conta não pode ser nulo!"));
                var value = await _context.SaveChangesAsync();

                if (value == 1)
                {
                    return Ok("Conta atualzada com sucesso!");
                }
                else
                {
                    return BadRequest("A conta não foi atualizada");
                }
            }
            catch (NullReferenceException ne)
            {
                return NotFound(ne.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                if (id > 0)
                {
                    Account account = await _context.Account.FindAsync(id) ?? throw new NullReferenceException("Conta não encontrada!");
                    _context.Account.Remove(account);
                    var value = await _context.SaveChangesAsync();

                    if (value == 1)
                    {
                        return Ok("Conta deletada com sucesso!");
                    }
                    else
                    {
                        return BadRequest("A conta não foi deletada com sucesso!");
                    }
                }

                return NotFound("Conta tem que ser maior que 0");

            }
            catch (NullReferenceException ne)
            {
                return NotFound(ne.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}
