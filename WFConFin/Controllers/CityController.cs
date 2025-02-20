using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using System.Reflection.Metadata;
using System.Xml.Serialization;
using WFConFin.Data;
using WFConFin.Models;

namespace WFConFin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CityController : Controller
    {
        private readonly WFConFinDbContext _context;

        public CityController(WFConFinDbContext context)
        {
            _context = context;
        }

        #region Get City
        [HttpGet]
        public async Task<IActionResult> GetCity()
        {
            try
            {
                var city = await _context.City.Include(c => c.State).OrderBy(c => c.Name).ToListAsync();
                return Ok(city);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error in City with message {ex.Message} with path {ex.StackTrace}");
            }
        }
        #endregion
        
        #region GetCityId
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCity([FromRoute] Guid id)
        {
            try
            {
                City city = await _context.City.FindAsync(id) ?? throw new NullReferenceException("City Not found");

                return Ok(city);
            }
            catch(NullReferenceException n)
            {
                return BadRequest(n.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error in City with message {ex.Message} with path {ex.StackTrace}");
            }
        }
        #endregion

        #region GetCityPagination
        [HttpGet("Pagination")]
        public async Task<IActionResult> GetCityPagination([FromQuery] string? value, int skip, int take, bool desc)
        {
            try
            {
                var city = await _context.City.ToListAsync();

                _ = city.FirstOrDefault() ?? throw new NullReferenceException("City not found");

                if(!value.IsNullOrEmpty())
                {
                    value = value.ToUpper();
                    city =  city.Where(o => o.Name.ToUpper().Contains(value) || o.StateSigla.ToUpper().Contains(value)).ToList();
                }


                if (desc)
                {
                    city = city.OrderByDescending(c => c.Name).ToList();
                }
                else
                {
                    city = city.OrderBy(c => c.Name).ToList();
                }

                int amount = city.Count();

                city = city.Skip((--skip) * take).Take(take).ToList();

                var pr = new PaginationResponse<City>(city, amount, skip, take);

                return Ok(pr);
            }
            catch(NullReferenceException n)
            {
                return BadRequest(n.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error in GetStatePagination With message: {ex.Message} with path: {ex.StackTrace}");
            }
        }
        #endregion

        #region GetSiglaSmart
        [HttpGet("Search")]
        public async Task<IActionResult> GetSiglaSeach([FromQuery] string value)
        {
            try
            {
                value = value.ToUpper();

                var city = await _context.City.Where(c => c.Name.ToUpper().Contains(value) || c.StateSigla.ToUpper().Contains(value)).ToListAsync();

                _ = city.FirstOrDefault() ?? throw new NullReferenceException($"Estado não existe no banco de dados");

                return Ok(city);
            }
            catch(NullReferenceException n)
            {
                return BadRequest(n.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error in GetSiglaSearch With message: {ex.Message} with path: {ex.StackTrace}");
            }
        }
        #endregion

        #region Post City
        [HttpPost]
        public async Task<IActionResult> PostCity([FromBody] City city)
        {
            
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                city.StateSigla = city.StateSigla.ToUpper();
                city.Name = city.Name.ToUpper();

                await _context.City.AddAsync(city);
                var value = await _context.SaveChangesAsync();

                if (value == 1)
                {
                    return Ok("The city was successfully registered");
                }
                else
                {
                    return BadRequest("Problem: City don't registered");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error in City with message {ex.Message} with path {ex.StackTrace}");
            }
        }
        #endregion

        #region Put City
        [HttpPut]
        public async Task<IActionResult> PutCity([FromBody] City city)
        {
            try
            {
                if(city is null)
                {
                    throw new ArgumentNullException("Cidade Não pode ser nula");
                }

                _context.City.Update(city);
                int value = await _context.SaveChangesAsync();

                if (value == 1)
                {
                    return Ok("The city was Updated");
                }
                else
                {
                    return BadRequest("Problem: City don't Updated");
                }
            }
            catch(ArgumentNullException ae)
            {
                return NotFound(ae.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error in City with message {ex.Message} with path {ex.StackTrace}");
            }
        }
        #endregion

        #region DeleteCity
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCity([FromRoute] Guid id)
        {
            try
            {
                City city = await _context.City.FindAsync(id) ?? throw new NullReferenceException("City not Found");

                _context.City.Remove(city);

                int value = await _context.SaveChangesAsync();

                if (value == 1)
                {
                    return Ok("The city was Deleted");
                }
                else
                {
                    return BadRequest("Problem: City don't Deleted");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error in City with message {ex.Message} with path {ex.StackTrace}");
            }
        }
        #endregion

    }
}
