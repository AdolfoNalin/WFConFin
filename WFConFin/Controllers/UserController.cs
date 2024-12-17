using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using WFConFin.Data;
using WFConFin.Models;
using WFConFin.Services;

namespace WFConFin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly WFConFinDbContext _context;
        private readonly TokenService _tokenService;

        public UserController(WFConFinDbContext context, TokenService tokenService)
        {
            _tokenService = tokenService;
            _context = context;
        }

        #region Login
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserLogin login)
        {
            try
            {
                login.Login = login.Login.ToUpper();

                string hashPassword = GeneratePassword(login.Password);

                User user = await _context.User.Where<User>(u => u.Login.ToUpper().Contains(login.Login)).FirstOrDefaultAsync() ??
                                                        throw new NullReferenceException("Login não realizado. Por favor verifique a senha ou o login");

                if(!user.Password.Equals(hashPassword))
                {
                    throw new InvalidOperationException("Senha incorreta");
                }

                var token = _tokenService.GenerateToken(user);
                user.Password = "";

                var result = new UserResponse
                {
                    User = user,
                    Token = token
                };

                return Ok(result);
            }
            catch(InvalidOperationException ip)
            {
                return BadRequest(ip.Message);
            }
            catch(NullReferenceException ne)
            {
                return NotFound(ne.Message);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }
        #endregion

        #region GetAll
        [HttpGet]
        [Authorize(Roles = "Gerente, Empregado, Operador")]
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
        [Authorize(Roles = "Gerente, Empregado")]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            try
            {
                user.Login = user.Login.ToUpper();
                user.Password = user.Password.ToUpper(); 

                user.Password = GeneratePassword(user.Password);

                var users = await _context.User.Where<User>(u => u.Login.ToUpper().Contains(user.Login)).ToListAsync();

                if(users.Count() > 0)
                {
                    return BadRequest("Login do usuário já é existente");
                }
                else if (!ModelState.IsValid)
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
        [Authorize(Roles = "Gerente, Empregado")]
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
        [Authorize(Roles = "Gerente")]
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
    
        #region HashPassword
        private string GeneratePassword(string password)
        {
            string hashPassword;
            using(SHA256 pass = SHA256.Create())
            {
                byte[] Bpassword = Encoding.UTF8.GetBytes(password);
                byte[] bytePass = pass.ComputeHash(Bpassword);
                hashPassword = BitConverter.ToString(bytePass).Replace("-",".").ToLower();
            }

            return hashPassword;
        }
        #endregion
    }
}
