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
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly WFConFinDbContext _context;

        public UserController(WFConFinDbContext context)
        {
            _context = context;
        }

        #region GetName
        [HttpGet("{name}")]
        public async Task<IActionResult> GetName([FromRoute] string name)
        {
            try
            {
                name = name.ToUpper();
                return Ok(await _context.User.FindAsync(name) ?? throw new NullReferenceException("Usuário não existe no banco de dados"));
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

        #region GetNameSmart
        [HttpGet]
        public async Task<IActionResult> GetNameSmart([FromQuery] string value)
        {
            try
            {
                value = value.ToUpper();   
                List<User> users = await _context.User.Where(u => u.Name.ToUpper().Contains(value)).ToListAsync<User>();
                _ = users.FirstOrDefault() ?? throw new NullReferenceException("Usuário não existe no banco de dados");

                return Ok(users);
            }
            catch(NullReferenceException ne)
            {
                return NotFound(ne.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Could not find {ex.Message}");
            }
        }
        #endregion

        #region GetPagination
        [HttpGet]
        public async Task<IActionResult> GetPagination([FromQuery] string value, int skip, int take, bool desc)
        {
            try
            {
                value = value.ToUpper();
                var users = from u in await _context.User.ToListAsync() where u.Name.ToUpper().Contains(value) select u;

                _ = users.FirstOrDefault() ?? throw new NullReferenceException("Usuário não encontado no banco de dados");

                if(desc)
                {
                    users = from u in users orderby u.Name descending select u;
                }
                else
                {
                    users = from u in users orderby u.Name ascending select u;
                }

                int amount = users.Count();

                users.Skip(skip).Take(take).ToList<User>();

                var pr = new PaginationResponse<User>(users, amount, skip, take);
                return Ok(pr);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region GetAll
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(_context.User.OrderBy(u => u.Name).ToList<User>());
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

                _context.User.AddAsync(user);
                var value = await _context.SaveChangesAsync();

                if(value == 1)
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
                    return Ok("Usuário foi cadastrado com sucesso!");
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
