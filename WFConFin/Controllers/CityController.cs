using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        #region GetSiglaPagination
        [HttpGet("Pagination")]
        public async Task<IActionResult> GetCityPagination([FromQuery] string value, int skip, int take, bool desc)
        {
            try
            {
                value = value.ToUpper();

                var city = from o in await _context.City.ToListAsync()
                            where o.Name.ToUpper().Contains(value) || o.StateSigla.ToUpper().Contains(value)
                            select o;

                _ = city.FirstOrDefault() ?? throw new NullReferenceException("City not found");

                if (desc)
                {
                    city = from o in city orderby o.Name descending select o;
                }
                else
                {
                    city = from o in city orderby o.Name ascending select o;
                }

                int amount = city.Count();

                city = city.Skip(skip).Take(take).ToList();

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
                City obj = await _context.City.FindAsync(city.Id) ?? throw new NullReferenceException("City not found");

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
