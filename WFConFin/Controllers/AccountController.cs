using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Immutable;
using System.Data;
using WFConFin.Data;
using WFConFin.Models;

namespace WFConFin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AccountController : Controller
    {
        private WFConFinDbContext _context;

        public AccountController(WFConFinDbContext context)
        {
            _context = context;
        }

        #region GetPagination
        [HttpGet("Pagination")]
        [Authorize]
        public async Task<IActionResult> GetPagination([FromQuery] string? value, int skip, int take, bool desc)
        {
            try
            {
                var account = from o in await _context.Account.ToListAsync() select o;

                if(!value.IsNullOrEmpty())
                {
                    value = value.ToUpper();
                    account = from o in account
                              where o.Description.ToUpper().Contains(value) 
                      select o;
                }

                _ = account.FirstOrDefault() ?? throw new NullReferenceException($"Contas não existe no banco de dados");

                if (desc)
                {
                    account = from o in account orderby o.Description descending select o;
                }
                else
                {
                    account = from o in account orderby o.Description ascending select o;
                }

                int amount = account.Count();

                account = account.Skip((--skip) * take).Take(take).ToList();

                var pr = new PaginationResponse<Account>(account, amount, skip, take);

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

        #region GetSmartDescription
        [HttpGet("search")]
        [Authorize]
        public async Task<IActionResult> GetSmartDescription([FromQuery] string value)
        {
            try
            { 
                value = value.ToUpper();
                var account = await _context.Account.Where(a => a.Description.ToUpper().Contains(value)).ToListAsync();
                _ = account ?? throw new NullReferenceException("Nenhuma conta foi encontrada!");
                
                return Ok(account);
            }
            catch(NullReferenceException ne)
            {
                return NotFound(ne.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region GetId
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetId([FromRoute] Guid id)
        {
            try
            {
                var account = await _context.Account.FindAsync(id) ?? throw new NullReferenceException("Nenhuma conta encontrada!");

                return Ok(account);
            }
            catch(NullReferenceException ne)
            {
                return NotFound(ne.Message);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region GetAll
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _context.Account.OrderBy(x => x.Id).ToListAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Post
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] Account account)
        {
            try
            {
                DateTime date = new DateTime();
                account.DueDate = account.DueDate.ToUniversalTime();

                if (account.PaymentDate != null)
                {
                    date = account.PaymentDate.Value;
                    account.PaymentDate = date.ToUniversalTime();
                }

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
        [Authorize]
        public async Task<IActionResult> Put([FromBody] Account account)
        {
            try
            {
                DateTime date = new DateTime();
                account.DueDate = account.DueDate.ToUniversalTime();

                if(account.PaymentDate != null)
                {
                    date = account.PaymentDate.Value;
                    account.PaymentDate = date.ToUniversalTime();
                }
                
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
        [Authorize]
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
